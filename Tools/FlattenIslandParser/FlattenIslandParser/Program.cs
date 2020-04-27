using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using Newtonsoft.Json;


namespace SheetsQuickstart
{
    
    public class SuggestionOption {
    public string idSuggestionOption; // datas: 1 or 2
    public string text; // data: column B, second and third cell of the pattern
    public string money; // data: column T
    public string publicOp; // data: column U
    public string growthRate; // data: column V
    public string capacity; // data: column w
    public string vacune; // data: column X
    public string storyId; // data: column S
    public string startStoryId; // data: column Y
    public string stopStoryId; // data: column Z
    public SuggestionOption() {
        this.idSuggestionOption="";
        this.text="";
        this.money="";
        this.growthRate="";
        this.capacity="";
        this.vacune="";
        this.storyId="";
        this.startStoryId="";
        this.stopStoryId="";
    }
    }
    public class GamePhaseRequirement {
        public List<string> phaseId;
        public GamePhaseRequirement() {
            this.phaseId = new List<string>();
        }
        public List<string> getPhaseId(){
            return this.phaseId;
        }
    }
    /**
    * object with data of idStory
    */
    public class GameStoryRequirement {
        public string storyId;
        public GameStoryRequirement() {
            this.storyId="";
        }
        public string getStoryId(){
            return this.storyId;
        }
    }

    /**
    * Object that gets de data of the suggestion that 
    * will be sent to the xml file.
    * Includes suggestionOption and gamePhaseRequirement object
    **/
    public class SuggestionSheet
	{
		public List<SuggestionOption> suggestionOption;
		public GamePhaseRequirement gamePhaseRequirement;
		public GameStoryRequirement gameStoryRequirement;
		public string idSuggestion; // column A
		public string advisorId; // column M
		public string title; // column B, first cell of the pattern
		public string description;

		public SuggestionSheet()
		{
			this.suggestionOption = new List<SuggestionOption>(2);
			this.gamePhaseRequirement = new GamePhaseRequirement();
			this.gameStoryRequirement = new GameStoryRequirement();
			this.idSuggestion="";
			this.advisorId="";
			this.title="";
			this.description="";
    }

    public string getId()
	{
        return this.idSuggestion;
    }
}

    public class CSVConfigData
	{
        public string pathFile;

		public CSVConfigData()
		{
            this.pathFile = "";
        }
    }

    public class GSConfigData
	{
		public string credentialsPathFile;
        public string pathSheet;
        public string spreadSheedName;
        public string cellToExclude;


        public GSConfigData()
		{
			this.credentialsPathFile = "";
			this.pathSheet = "";
            this.spreadSheedName = "";
            this.cellToExclude = "";
        }
    } 

    public class ConfigData
	{
        public string CSVData;
        public string OptionMode;
		public string outputPathFile;

        public CSVConfigData cSVConfigData;

        public GSConfigData gSConfigData;
        public ConfigData(){
            this.OptionMode = "";
			this.outputPathFile = "";
            this.cSVConfigData = new CSVConfigData();
            this.gSConfigData = new GSConfigData();
        }
    }

    class Program
    {
        
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        

        static void Main(string[] args)
        {
            string configPath = "../../../input/config.json";

            string configFileData = File.ReadAllText(configPath);
            
            var dataConfig = JsonConvert.DeserializeObject<ConfigData>(configFileData);


            List<SuggestionSheet> suggestionList=new List<SuggestionSheet>();

            /**
            if value of opt is 1, reading in csv
            if value of opt is 2, reading from googlesheet
            **/
            string opt=dataConfig.OptionMode;
            Console.WriteLine("opt: "+opt);
            if (opt == "1")
            {
                Console.WriteLine("entramos en csv\n\n");    
                List<List<string>> valuesCSV= new List<List<string>>();
                valuesCSV = ReadingCSV();
                FeedingnObjectFromCSVValue(valuesCSV);

            }else if(opt == "2"){
                Console.WriteLine("entramos en GS\n\n");
                IList<IList<Object>> valuesSheet =  ReadingFromGoogleSheets();
                FeedingnObjectFromSheetsValue(valuesSheet);
            }else{
                Console.WriteLine("varialbe opt should have values :");
                Console.WriteLine("1 - reading from CSV file");
                Console.WriteLine("2 - reading from googlesheet");
            }

            createXmlFile(suggestionList);
            
            /***************************************************************/
            /**METHODS
            List of methods used in the project
            **/

            void createXmlFile(List<SuggestionSheet> suggestions) {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                using (XmlWriter writer = XmlWriter.Create(dataConfig.outputPathFile, settings))  
                {
                    writer.WriteStartElement("group");  
                    int suggestionIndex=0;
                    foreach(SuggestionSheet suggestion in suggestions)
					{
                        writer.WriteStartElement("suggestion");  
                        writer.WriteAttributeString("id", suggestion.idSuggestion);
                        writer.WriteAttributeString("advisorId", suggestion.advisorId);
                        writer.WriteAttributeString("title", suggestion.title);        
                        writer.WriteAttributeString("description", suggestion.description);        
                        writer.WriteStartElement("suggestionOptions");

						foreach (SuggestionOption suggestionOpt in suggestion.suggestionOption)
						{
                            writer.WriteStartElement("suggestionOption");
                            writer.WriteAttributeString("id", suggestionOpt.idSuggestionOption);
                            writer.WriteAttributeString("text", suggestionOpt.text);
                            if(suggestionOpt.money!="") writer.WriteAttributeString("money", suggestionOpt.money);
                            if(suggestionOpt.publicOp!="") writer.WriteAttributeString("publicOpinion", suggestionOpt.publicOp);
                            if(suggestionOpt.growthRate!="") writer.WriteAttributeString("growthRate", suggestionOpt.growthRate);
                            if(suggestionOpt.capacity!="") writer.WriteAttributeString("capacity", suggestionOpt.capacity);
                            if(suggestionOpt.vacune!="") writer.WriteAttributeString("vaccine", suggestionOpt.vacune);
                            if(suggestionOpt.startStoryId!="") writer.WriteAttributeString("startStoryId", suggestionOpt.startStoryId);
                            if(suggestionOpt.stopStoryId!="") writer.WriteAttributeString("stopStoryId", suggestionOpt.stopStoryId);
                            writer.WriteEndElement();  
                            suggestionIndex++;
						}

                        writer.WriteEndElement();  
                        writer.WriteStartElement("requirements");
						writer.WriteStartElement("gamePhaseRequirement");

						foreach (string idPhase in suggestion.gamePhaseRequirement.phaseId)
                        {
                            writer.WriteStartElement("phaseId");
                            writer.WriteString(idPhase);
                            writer.WriteEndElement(); 
                        }

						writer.WriteEndElement();

						if (suggestion.gameStoryRequirement.storyId != "")
						{
							writer.WriteStartElement("gameStoryRequirement");  
                            writer.WriteStartElement("storyId");  
                            writer.WriteString(suggestion.gameStoryRequirement.storyId);
                            writer.WriteEndElement();
							writer.WriteEndElement();
						}

						writer.WriteEndElement();
                        writer.WriteEndElement();  
                    }

					writer.WriteEndElement();  
                }
            }

            SuggestionSheet fillObjectFromGS(IList<IList<Object>> ss, int row)
			{
                int lastCellSuggestionRow=(ss[(row)].Count)-1;
                SuggestionSheet suggestionSheet = new SuggestionSheet();
                suggestionSheet.idSuggestion=ss[row][0].ToString();
                
                suggestionSheet.advisorId=
                ss[row][2].ToString() == "PR"? "2001":
                ss[row][2].ToString() == "Treasurer"? "2002":
                ss[row][2].ToString() == "Lab Specialist"? "2003":
                ss[row][2].ToString() == "Hospital Manager"? "2004":
                ss[row][2].ToString() == "Commander"? "2005":""
                ;
                suggestionSheet.title=ss[row][1].ToString();
                suggestionSheet.description=ss[(row+1)][1].ToString();
                for(int i = 0; i < 2; i++){
                    SuggestionOption aux = new SuggestionOption();
                    int newPosition = row+i+2;
                    int lastCellSuggestionOptionRow=(ss[(newPosition)].Count)-1;
                    aux.idSuggestionOption = (i+1).ToString();

                    /**
                    * A-0 | B-1 | C-2 | D-3 | E-4 | F-5 | G-6 | H-7 | I-8 | J-9 | K-10 |
                    L-12 | M-12 | N-13 | O-14 | P-15 | Q-16 | R-17 | S-18 | T-19 | U-20 
                    V-21 | W-22 | X-23 | Y-24 | Z-25 
                    */
                    aux.text=ss[newPosition][1].ToString() != "" ? ss[newPosition][1].ToString(): "";

                    aux.money=lastCellSuggestionOptionRow < 8 ? "": ss[newPosition][8].ToString();

                    aux.publicOp = lastCellSuggestionOptionRow < 9 ? "": ss[newPosition][9].ToString();

                    aux.growthRate = lastCellSuggestionOptionRow < 10 ? "": ss[newPosition][10].ToString();

                    aux.capacity = lastCellSuggestionOptionRow < 11 ? "": ss[newPosition][11].ToString();

                    aux.vacune = lastCellSuggestionOptionRow < 12 ? "": ss[newPosition][12].ToString();

                    aux.startStoryId = lastCellSuggestionOptionRow < 13 ? "": ss[newPosition][13].ToString();
                    aux.stopStoryId = lastCellSuggestionOptionRow < 14 ? "": ss[newPosition][14].ToString();
                    suggestionSheet.suggestionOption.Add(aux);
                }
                Console.WriteLine("last cell row :"+lastCellSuggestionRow);
                suggestionSheet.gameStoryRequirement.storyId = lastCellSuggestionRow < 7?"":ss[row][7].ToString()==""?"":ss[row][7].ToString();
                if(ss[row][4].ToString() == "TRUE") suggestionSheet.gamePhaseRequirement.phaseId.Add("1");
                if(ss[row][5].ToString() == "TRUE") suggestionSheet.gamePhaseRequirement.phaseId.Add("2");
                if(ss[row][6].ToString() == "TRUE") suggestionSheet.gamePhaseRequirement.phaseId.Add("3");
                if(ss[row][3].ToString() == "TRUE") suggestionSheet.gamePhaseRequirement.phaseId.AddRange(new string[]{"1","2","3"});
                return suggestionSheet;
            }

            SuggestionSheet FillObjectFromCSV(List<List<string>> ss, int row)
			{

				Console.WriteLine("row "+row+" | ");
                int lastCellRow=0;
                SuggestionSheet suggestionSheet = new SuggestionSheet();
                suggestionSheet.idSuggestion=ss[row][0];

				suggestionSheet.advisorId =
                	ss[row][2] == "PR"? "2001":
					ss[row][2] == "Treasurer"? "2002":
					ss[row][2] == "Lab Specialist"? "2003":
					ss[row][2] == "Hospital Manager"? "2004":
					ss[row][2] == "Commander"? "2005":"";

                suggestionSheet.title=ss[row][1];
                suggestionSheet.description=ss[(row+1)][1];

				for (int i = 0; i < 2; i++)
				{
                    SuggestionOption aux = new SuggestionOption();
                    int newPosition = row+i+2;
                    lastCellRow=(ss[newPosition].Count)-1;
                    aux.idSuggestionOption = (i+1).ToString();

                    /**
                    * A-0 | B-1 | C-2 | D-3 | E-4 | F-5 | G-6 | H-7 | I-8 | J-9 | K-10 |
                    L-12 | M-12 | N-13 | O-14 | P-15 | Q-16 | R-17 | S-18 | T-19 | U-20 
                    V-21 | W-22 | X-23 | Y-24 | Z-25 
                    */
                    aux.text=ss[newPosition][1] != "" ? ss[newPosition][1]: "";

                    aux.money=lastCellRow < 8 ? "": ss[newPosition][8];

                    aux.publicOp = lastCellRow < 9 ? "": ss[newPosition][9];

                    aux.growthRate = lastCellRow < 10 ? "": ss[newPosition][10];

                    aux.capacity = lastCellRow < 11 ? "": ss[newPosition][11];

                    aux.vacune = lastCellRow < 12 ? "": ss[newPosition][12];

                    aux.startStoryId = lastCellRow < 13 ? "": ss[newPosition][13];
                    aux.stopStoryId = lastCellRow < 14 ? "": ss[newPosition][14];
                    suggestionSheet.suggestionOption.Add(aux);
                }

                suggestionSheet.gameStoryRequirement.storyId = ss[row][7]==""?"":ss[row][7];

				if (ss[row][3] == "TRUE") suggestionSheet.gamePhaseRequirement.phaseId.Add("0");
				if (ss[row][4] == "TRUE") suggestionSheet.gamePhaseRequirement.phaseId.Add("1");
                if(ss[row][5] == "TRUE") suggestionSheet.gamePhaseRequirement.phaseId.Add("2");
                if(ss[row][6] == "TRUE") suggestionSheet.gamePhaseRequirement.phaseId.Add("3");

				return suggestionSheet;
            }
    
            List<List<string>> ReadingCSV()
			{
                string path = dataConfig.cSVConfigData.pathFile;
                List<List<string>> values= new List<List<string>>();
                string[] readText = File.ReadAllLines(path);
                foreach(string text in readText)
				{
                    List<string> row = new List<string>();
                    foreach (string value in text.Split(','))
					{
                        row.Add(value);
                    }

					values.Add(row);
                }

                for (int i=0;i<values.Count;i++)
                {   
                    for(int j=0;j<values[i].Count;j++)
                    {
                        Console.Write("cellll "+j+" : "+values[i][j].ToString()+"; ");

                    }
                    Console.WriteLine("\n-----------------------------------------");
                    
                }
                return values;
            }

            IList<IList<Object>> ReadingFromGoogleSheets()
			{
                string ApplicationName = "Google Sheets API .NET Quickstart";
                string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
                UserCredential credential;
                using (var stream =
                    new FileStream(dataConfig.gSConfigData.credentialsPathFile, FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, false)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Google Sheets API service.
                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Define request parameters.
                String spreadsheetId = dataConfig.gSConfigData.pathSheet;
                String range = dataConfig.gSConfigData.spreadSheedName;
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);
                // SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.G(spreadsheetId);

                // Prints the names and majors of students in a sample spreadsheet:
                // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
                ValueRange response = request.Execute();
                return response.Values;
            }
       
            List<SuggestionSheet> FeedingnObjectFromSheetsValue(IList<IList<Object>> values)
			{
				if (values != null && values.Count > 0)
				{
					suggestionList = new List<SuggestionSheet>(10);
                
					for (int i=0;i<values.Count;i++)
					{   
						Console.WriteLine("column : "+i);
						for(int j=0;j<values[i].Count;j++)
						{
							Console.Write("cell "+j+" : "+values[i][j].ToString()+"; ");

						}
						Console.WriteLine("\n-----------------------------------------");
					}

					int rowId=0;
					for (int i=0;i<values.Count;i++)
					{
						Console.WriteLine("columna :"+i);
						if(values[i][0].ToString() != dataConfig.gSConfigData.cellToExclude && values[i][0].ToString() != "")
						{
							SuggestionSheet suggest = new SuggestionSheet();
							suggest=fillObjectFromGS(values, i);
							suggestionList.Add(suggest);
						}
						else
						{

						}
						rowId++;
					}  
					return suggestionList;
				}
				else
				{
					Console.WriteLine("No data found.");
					Console.Read();
					return null;
				}
            }
            
            List<SuggestionSheet> FeedingnObjectFromCSVValue(List<List<string>> values){
            if (values != null && values.Count > 0)
            {
                suggestionList = new List<SuggestionSheet>(10);
                int rowId=0;
                for (int i=0;i<values.Count;i++)
                {
                    if(values[i][0].ToString() != "Card ID (3001-3999)" && values[i][0].ToString() != "")
                    {
                        SuggestionSheet suggest = new SuggestionSheet();
                        suggest=FillObjectFromCSV(values, i);
                        suggestionList.Add(suggest);
                    }

					rowId++;
                }  
                return suggestionList;
            }
            else
            {
                Console.WriteLine("No data found.");
                Console.Read();
                return null;
            }
            }
            
  
            
        }
    }
}