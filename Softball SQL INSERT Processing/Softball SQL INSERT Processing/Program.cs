using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
class Program
{
    static void Main()
    {
        string filePath = "Softball Match Data 99-22.csv"; // Update the file path as needed
        string[] lines = File.ReadAllLines(filePath);

        int oppIdCounter = 1;
        int locIdCounter = 1;
        int trnIdCounter = 1;
        //int mchIdCounter = 1;

        Dictionary<string, string> oppDict = new Dictionary<string, string>();
        Dictionary<string, string> locDict = new Dictionary<string, string>();
        Dictionary<string, string> trnDict = new Dictionary<string, string>();
        Dictionary<int, string> mchDict = new Dictionary<int, string>();

        StringBuilder OppInsertStatements = new StringBuilder();
        StringBuilder LocInsertStatements = new StringBuilder();
        StringBuilder TrnInsertStatements = new StringBuilder();
        StringBuilder MchInsertStatements = new StringBuilder();

        OppInsertStatements.AppendLine($"USE BUDT703_Project_0507_01\n");
        OppInsertStatements.AppendLine($"INSERT INTO [CurveBallConsultants.Opponent] (oppId, oppName) VALUES");

        LocInsertStatements.AppendLine($"USE BUDT703_Project_0507_01\n");
        LocInsertStatements.AppendLine($"INSERT INTO [CurveBallConsultants.Location] (locId, locCity, locState) VALUES");

        TrnInsertStatements.AppendLine($"USE BUDT703_Project_0507_01\n");
        TrnInsertStatements.AppendLine($"INSERT INTO [CurveBallConsultants.Tournament] (trnId, trnName) VALUES");

        MchInsertStatements.AppendLine($"USE BUDT703_Project_0507_01\n");
        MchInsertStatements.AppendLine($"INSERT INTO [CurveBallConsultants.Match] (oppId, locId, mchDate, mchTime, mchType, runTerps, runOpponent, trnId, ssnYear) VALUES");

        //FOR OPPONENT, LOCATION, TOURNAMENT, SEASON INSERT STATEMENTS
        foreach (var line in lines.Skip(1)) // Start from 1 to skip the header line
        {

            string[] columns = line.Split(',');
            if (columns[7] != "Cancelled" && columns[7] != "Postponed")
            {
                string oppId = oppIdCounter.ToString("D3"); // Assuming oppId is generated incrementally
                string oppName = columns[3].Trim();

                // Generate SQL INSERT statement
                if (!oppDict.ContainsKey(oppId) && !oppDict.ContainsValue(oppName))
                {
                    oppDict.Add(oppId, oppName);
                    OppInsertStatements.AppendLine($"('{oppId}', '{oppName}'),");
                    oppIdCounter++;
                }

                string locId = locIdCounter.ToString("D3"); // Assuming oppId is generated incrementally
                string locCity = columns[4].Trim();
                string locState = columns[5].Trim();

                // Generate SQL INSERT statement
                if (!locDict.ContainsKey(locId) && !locDict.ContainsValue($"{locCity} {locState}") && locCity != "TBD")
                {
                    locDict.Add(locId, $"{locCity} {locState}");
                    LocInsertStatements.AppendLine($"('{locId}', '{locCity}', '{locState}'),");
                    locIdCounter++;
                }

                string trnId = trnIdCounter.ToString("D3"); // Assuming oppId is generated incrementally
                string trnName = columns[6].Trim(); ;

                if (!trnDict.ContainsKey(trnId) && !trnDict.ContainsValue(trnName) && trnName != "")
                {
                    trnDict.Add(trnId, trnName);
                    TrnInsertStatements.AppendLine($"('{trnId}', '{trnName}'),");
                    trnIdCounter++;
                }

                string[] dateItems = columns[0].Trim().Split('-');

            }
        }

        // Print or save the SQL insert statements
        Console.WriteLine(OppInsertStatements.ToString());

        // You can also save the SQL insert statements to a file if needed
        File.WriteAllText("Project_0507_01_Opponent_INSERT.sql", OppInsertStatements.ToString());

        /*******************************************************************************************************************************************************/

        // Print or save the SQL insert statements
        Console.WriteLine(LocInsertStatements.ToString());

        // You can also save the SQL insert statements to a file if needed
        File.WriteAllText("Project_0507_01_Location_INSERT.sql", LocInsertStatements.ToString());

        /*******************************************************************************************************************************************************/

        // Print or save the SQL insert statements
        Console.WriteLine(TrnInsertStatements.ToString());

        // You can also save the SQL insert statements to a file if needed
        File.WriteAllText("Project_0507_01_Tournament_INSERT.sql", TrnInsertStatements.ToString());

        /*******************************************************************************************************************************************************/

        foreach (var line in lines.Skip(1)) // Start from 1 to skip the header line
        {

            string[] columns = line.Split(',');
            if (columns[7] != "Cancelled" && columns[7] != "Postponed" && columns[4] != "TBD")
            {
                string[] dateItems = columns[0].Trim().Split('-');
                string ssnYear = dateItems[0];
                string mchDate = columns[0].Trim();
                string mchTime = columns[1].Trim();
                string mchType = columns[2].Trim();
                string oppName = columns[3].Trim();
                string locCity = columns[4].Trim();
                string runTerps = columns[8];
                string runOpponents = columns[9];
                string trnName = columns[6].Trim();

                string oppId = "", locId = "", trnId = "";
                //string mchId = mchIdCounter.ToString("D4");

                foreach (var item in oppDict.Keys)
                {
                    if (oppDict[item] == oppName)
                    {
                        oppId = item;
                    }
                }

                foreach (var item in locDict.Keys)
                {
                    if (locDict[item].Contains(locCity))
                    {
                        locId = item;
                    }
                }

                foreach (var item in trnDict.Keys)
                {
                    if (trnDict[item].Contains(trnName))
                    {
                        trnId = item;
                    }
                }

                MchInsertStatements.AppendLine($"('{oppId}', '{locId}', '{mchDate}', '{mchTime}', '{mchType}', {runTerps}, {runOpponents}, '{trnId}', '{ssnYear}'),");
                //mchIdCounter++;
            }
        }

        // Print or save the SQL insert statements
        Console.WriteLine(MchInsertStatements.ToString());

        // You can also save the SQL insert statements to a file if needed
        File.WriteAllText("Project_0507_01_Match_INSERT.sql", MchInsertStatements.ToString());
    }
}