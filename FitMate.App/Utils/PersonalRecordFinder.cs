using Microsoft.Data.SqlClient;
using Nito.AsyncEx.Synchronous;

namespace FitMate.Utils;

public static class PersonalRecordFinder
{
    private static readonly string SINGLE_ID;
    private static readonly string ALL_IDS;

    static PersonalRecordFinder()
    {
        SINGLE_ID = "SELECT et.ID as type_id, MIN(e.ID) as exercise_id " +
                    "FROM Exercises e " +
                    "INNER JOIN ExerciseTypes et ON e.ExerciseTypeID = et.ID " +
                    "INNER JOIN ( " +
                    "  SELECT et2.ID, " +
                    "  MAX(IIF(" +
                    "    et2.MeasurementTypeID = 1," +
                    "    (e2.KgsOrMtr * e2.RepsOrSecs), " +
                    "    (ROUND((CAST(e2.KgsOrMtr AS float) / e2.RepsOrSecs), 1)))) AS total_max " +
                    "  FROM Exercises e2 " +
                    "  INNER JOIN ExerciseTypes et2 ON e2.ExerciseTypeID = et2.ID " +
                    "  GROUP BY et2.ID " +
                    ") exercise_totals ON et.ID = exercise_totals.ID " +
                    "AND IIF(" +
                    "  et.MeasurementTypeID = 1, " +
                    "  (e.KgsOrMtr * e.RepsOrSecs), " +
                    "  (ROUND((CAST(e.KgsOrMtr AS float) / e.RepsOrSecs), 1))) = exercise_totals.total_max " +
                    "WHERE et.ID = @etID " +
                    "GROUP BY et.ID ";

        ALL_IDS = SINGLE_ID.Replace("WHERE et.ID = @etID ", string.Empty);
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
    
    public static int[] FindAll()
    {
        SqlCommand command = new(ALL_IDS);
        List<int> ids = [];
        
        Task.Run(() => SqlCommunicator.Select(command, reader => { ids.Add(Convert.ToInt32(reader["exercise_id"])); }))
            .WaitAndUnwrapException();
        

        return [..ids];
    }
}