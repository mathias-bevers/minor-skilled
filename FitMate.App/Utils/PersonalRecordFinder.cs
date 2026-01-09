using System.Diagnostics;
using FitMate.Models;
using Microsoft.Data.SqlClient;
using Nito.AsyncEx.Synchronous;

namespace FitMate.Utils;

public static class PersonalRecordFinder
{
    private static readonly string SINGLE_ID;
    private static readonly string ALL_IDS;

    static PersonalRecordFinder()
    {
        SINGLE_ID = "SELECT MIN(e.ID) as exercise_id " + "FROM Exercises e " +
                    "INNER JOIN ExerciseTypes et ON e.ExerciseTypeID = et.ID " + "INNER JOIN ( " + "  SELECT et2.ID, " +
                    "  MAX(IIF(" + "    et2.MeasurementTypeID = 1," + "    (e2.KgsOrMtr * e2.RepsOrSecs), " +
                    "    (ROUND((CAST(e2.KgsOrMtr AS float) / e2.RepsOrSecs), 1)))) AS total_max " +
                    "  FROM Exercises e2 " + "  INNER JOIN ExerciseTypes et2 ON e2.ExerciseTypeID = et2.ID " +
                    "  GROUP BY et2.ID " + ") exercise_totals ON et.ID = exercise_totals.ID " + "AND IIF(" +
                    "  et.MeasurementTypeID = 1, " + "  (e.KgsOrMtr * e.RepsOrSecs), " +
                    "  (ROUND((CAST(e.KgsOrMtr AS float) / e.RepsOrSecs), 1))) = exercise_totals.total_max " +
                    "WHERE et.ID = @etID " + "GROUP BY et.ID ";

        ALL_IDS = "WITH pre_calculated AS( " +
                  "	SELECT " +
                  "		e.ID as id,  " +
                  "		e.ExerciseTypeID as type_id, " +
                  "		CASE WHEN et.MeasurementTypeID = 1  " +
                  "			THEN e.KgsOrMtr * e.RepsOrSecs  " +
                  "			ELSE ROUND(CONVERT(numeric, e.KgsOrMtr) / NULLIF(e.RepsOrSecs, 0), 1)  " +
                  "		END as score " +
                  "	FROM Exercises e " +
                  "	INNER JOIN ExerciseTypes et ON e.ExerciseTypeID = et.ID " +
                  "	INNER JOIN Workouts w ON e.WorkoutID = w.ID " +
                  "	INNER JOIN Users u ON w.UserID = u.ID " +
                  "	WHERE u.ID = @userID " +
                  "), " +
                  "max_per_type AS( " +
                  "	SELECT type_id, MAX(score) as score " +
                  "	FROM pre_calculated " +
                  "	GROUP BY type_id " +
                  "), " +
                  "pr_ids AS ( " +
                  "SELECT " +
                  "	MIN(pc.id) as id " +
                  "FROM " +
                  "	pre_calculated pc " +
                  "INNER JOIN max_per_type mpt ON " +
                  "	pc.type_id = mpt.type_id " +
                  "WHERE " +
                  "	pc.score = mpt.score " +
                  "GROUP BY " +
                  "	pc.type_id  " +
                  ") " +
                  "SELECT e.ID, e.KgsOrMtr, e.RepsOrSecs, et.Name, et.MeasurementTypeID FROM Exercises e " +
                  "INNER JOIN ExerciseTypes et ON e.ExerciseTypeID = et.ID " +
                  "INNER JOIN pr_ids ON e.ID = pr_ids.id ";
    }


    public static int FindForExerciseID(int exerciseID)
    {
        SqlCommand command = new(SINGLE_ID);
        command.Parameters.AddWithValue("@etID", exerciseID);
        int id = -1;

        Task.Run(() => SqlCommunicator.Select(command, reader => { id = Convert.ToInt32(reader["exercise_id"]); }))
            .WaitAndUnwrapException();

        return id > 0 ? id : throw new PopupException($"No PR id for exercise with id: {exerciseID}", "PR NOT FOUND");
    }

    public static Exercise[] FindAll()
    {
        SqlCommand command = new(ALL_IDS);
        command.Parameters.AddWithValue("@userID", App.USER_ID);
        
        List<Exercise> prs = [];

        Task.Run(() => SqlCommunicator.Select(command, reader =>
        {
            prs.Add(new Exercise
            {
                KgsOrMtr = Convert.ToInt32(reader["KgsOrMtr"]),
                RepsOrSecs = Convert.ToInt32(reader["RepsOrSecs"]),
                ExerciseType = new ExerciseType
                {
                    Name = Convert.ToString(reader["Name"]) ?? "null",
                    MeasurementTypeID = Convert.ToInt32(reader["MeasurementTypeID"])
                }
            });
        })).WaitAndUnwrapException();

        return [..prs];
    }
}