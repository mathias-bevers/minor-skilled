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
        SINGLE_ID = "WITH pre_calculated AS( " +
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
                    "	WHERE u.ID = @user_id AND et.ID = @exercise_type " +
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

        ALL_IDS = SINGLE_ID.Replace("AND et.ID = @exercise_type ", string.Empty);
    }


    public static int FindForExerciseID(int exerciseID)
    {
        SqlCommand command = new(SINGLE_ID);
        command.Parameters.AddWithValue("@user_id", App.USER_ID);
        command.Parameters.AddWithValue("@exercise_type", exerciseID);
        int id = -1;

        Task.Run(() => SqlCommunicator.Select(command, reader => { id = Convert.ToInt32(reader["ID"]); }))
            .WaitAndUnwrapException();

        return id > 0 ? id : throw new PopupException($"No PR id for exercise with id: {exerciseID}", "PR NOT FOUND");
    }

    public static Exercise[] FindAll(int userID)
    {
        SqlCommand command = new(ALL_IDS);
        command.Parameters.AddWithValue("@user_id", userID);
        
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