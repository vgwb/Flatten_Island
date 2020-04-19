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
    public class SuggestionSheet {
    public List<SuggestionOption> suggestionOption;
    public GamePhaseRequirement gamePhaseRequirement;
    public GameStoryRequirement gameStoryRequirement;
    public string idSuggestion; // column A
    public string advisorId; // column M
    public string title; // column B, first cell of the pattern
    public string description;
    public SuggestionSheet() {
        this.suggestionOption = new List<SuggestionOption>(2);
        this.gamePhaseRequirement = new GamePhaseRequirement();
        this.gameStoryRequirement = new GameStoryRequirement();
        this.idSuggestion="";
        this.advisorId="";
        this.title="";
        this.description="";
    }

    public string getId(){
        return this.idSuggestion;
    }
}

    public class CSVConfigData{
        public string pathFile;
        public CSVConfigData(){
            this.pathFile = "";
        }
    }

    public class GSConfigData{
        public string pathSheet;
        public string spreadSheedName;
        public string cellToExclude;


        public GSConfigData(){
            this.pathSheet = "";
            this.spreadSheedName = "";
            this.cellToExclude = "";
        }
    } 
    public class ConfigData{
        public string CSVData;
        public string OptionMode;

        public CSVConfigData cSVConfigData;

        public GSConfigData gSConfigData;
        public ConfigData(){
            this.OptionMode = "";
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
            string configPath = "config.json";

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
                valuesCSV = readingCSV();
                feedingnObjectFromCSVValue(valuesCSV);

            }else if(opt == "2"){
                Console.WriteLine("entramos en GS\n\n");
                IList<IList<Object>> valuesSheet =  readingFromGoogleSheets();
                feedingnObjectFromSheetsValue(valuesSheet);
            }else{
                Console.WriteLine("varialbe opt should have values :");
                Console.WriteLine("1 - reading from CSV file");
                Console.WriteLine("2 - reading from googlesheet");
            }

            crateXmlFile(suggestionList);
            
            /***************************************************************/
            /**METHODS
            List of methods used in the project
            **/

            void crateXmlFile(List<SuggestionSheet> suggestionList) {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                using (XmlWriter writer = XmlWriter.Create("../../Assets/09_Data/suggestions.xml", settings))  
                {
                    writer.WriteStartElement("group");  
                    int suggestionIndex=0;
                    foreach(SuggestionSheet suggestion in suggestionList)
                        {
                            writer.WriteStartElement("suggestion");  
                            writer.WriteAttributeString("id", suggestion.idSuggestion);
                            writer.WriteAttributeString("advisorId", suggestion.advisorId);
                            writer.WriteAttributeString("title", suggestion.title);        
                            writer.WriteAttributeString("description", suggestion.description);        
                            writer.WriteStartElement("suggestionOptions");
                                foreach(SuggestionOption suggestionOpt in suggestion.suggestionOption)
                                {
                                    writer.WriteStartElement("suggestionOption");
                                    writer.WriteAttributeString("id", suggestionOpt.idSuggestionOption);
                                    writer.WriteAttributeString("text", suggestionOpt.text);
                                    if(suggestionOpt.money!="") writer.WriteAttributeString("money", suggestionOpt.money);
                                    if(suggestionOpt.publicOp!="") writer.WriteAttributeString("publicOpinion", suggestionOpt.publicOp);
                                    if(suggestionOpt.growthRate!="") writer.WriteAttributeString("growthRate", suggestionOpt.growthRate);
                                    if(suggestionOpt.capacity!="") writer.WriteAttributeString("capacity", suggestionOpt.capacity);
                                    if(suggestionOpt.vacune!="") writer.WriteAttributeString("vacune", suggestionOpt.vacune);
                                    if(suggestionOpt.startStoryId!="") writer.WriteAttributeString("startStoryId", suggestionOpt.startStoryId);
                                    if(suggestionOpt.stopStoryId!="") writer.WriteAttributeString("stopStoryId", suggestionOpt.stopStoryId);
                                    writer.WriteEndElement();  
                                    suggestionIndex++;
                                }
                            writer.WriteEndElement();  
                            writer.WriteStartElement("requirements");  
                                writer.WriteStartElement("gamePhaseRequirement");  
                                    foreach(string idPhase in suggestion.gamePhaseRequirement.phaseId)
                                    {
                                        writer.WriteStartElement("phaseId");
                                        writer.WriteString(idPhase);
                                        writer.WriteEndElement(); 
                                    }
                                writer.WriteEndElement(); 
                                writer.WriteStartElement("gameStoryRequirement");  
                                    if(suggestion.gameStoryRequirement.storyId != ""){
                                        writer.WriteStartElement("storyId");  
                                        writer.WriteString(suggestion.gameStoryRequirement.storyId);
                                        writer.WriteEndElement();  
                                    }
                                    
                                writer.WriteEndElement();  
                            writer.WriteEndElement();
                            writer.WriteEndElement();  
                        }
                    writer.WriteEndElement();  
                }
            }

            SuggestionSheet fillObjectFromGS(IList<IList<Object>> ss, int row) {
                int lastCellRow=0;
                SuggestionSheet sug = new SuggestionSheet();
                sug.idSuggestion=ss[row][0].ToString();
                sug.advisorId=ss[row][12].ToString();
                sug.title=ss[row][1].ToString();
                sug.description=ss[(row+1)][1].ToString();
                for(int i = 0; i < 2; i++){
                    SuggestionOption aux = new SuggestionOption();
                    int newPosition = row+i+2;
                    lastCellRow=(ss[newPosition].Count)-1;
                    aux.idSuggestionOption = (i+1).ToString();

                    /**
                    * A-0 | B-1 | C-2 | D-3 | E-4 | F-5 | G-6 | H-7 | I-8 | J-9 | K-10 |
                    L-12 | M-12 | N-13 | O-14 | P-15 | Q-16 | R-17 | S-18 | T-19 | U-20 
                    V-21 | W-22 | X-23 | Y-24 | Z-25 
                    */
                    aux.text=ss[newPosition][1].ToString() != "" ? ss[newPosition][1].ToString(): "";

                    aux.money=lastCellRow < 19 ? "": ss[newPosition][19].ToString();

                    aux.publicOp = lastCellRow < 20 ? "": ss[newPosition][20].ToString();

                    aux.growthRate = lastCellRow < 21 ? "": ss[newPosition][21].ToString();

                    aux.capacity = lastCellRow < 22 ? "": ss[newPosition][22].ToString();

                    aux.vacune = lastCellRow < 23 ? "": ss[newPosition][23].ToString();

                    aux.startStoryId = lastCellRow < 24 ? "": ss[newPosition][24].ToString();
                    aux.stopStoryId = lastCellRow < 25 ? "": ss[newPosition][25].ToString();
                    sug.suggestionOption.Add(aux);
                }
                sug.gameStoryRequirement.storyId = ss[row][13].ToString()=="FALSE"?"":ss[row][13].ToString();
                if(ss[row][15].ToString() == "TRUE") sug.gamePhaseRequirement.phaseId.Add("1");
                if(ss[row][16].ToString() == "TRUE") sug.gamePhaseRequirement.phaseId.Add("2");
                if(ss[row][17].ToString() == "TRUE") sug.gamePhaseRequirement.phaseId.Add("3");
                if(ss[row][14].ToString() == "TRUE") sug.gamePhaseRequirement.phaseId.AddRange(new string[]{"1","2","3"});
                return sug;
            }

            SuggestionSheet fillObjectFromCSV(List<List<string>> ss, int row) {
                int lastCellRow=0;
                SuggestionSheet sug = new SuggestionSheet();
                sug.idSuggestion=ss[row][0];
                sug.advisorId=ss[row][12];
                sug.title=ss[row][1];
                sug.description=ss[(row+1)][1];
                for(int i = 0; i < 2; i++){
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

                    aux.money=lastCellRow < 19 ? "": ss[newPosition][19];

                    aux.publicOp = lastCellRow < 20 ? "": ss[newPosition][20];

                    aux.growthRate = lastCellRow < 21 ? "": ss[newPosition][21];

                    aux.capacity = lastCellRow < 22 ? "": ss[newPosition][22];

                    aux.vacune = lastCellRow < 23 ? "": ss[newPosition][23];

                    aux.startStoryId = lastCellRow < 24 ? "": ss[newPosition][24];
                    aux.stopStoryId = lastCellRow < 25 ? "": ss[newPosition][25];
                    sug.suggestionOption.Add(aux);
                }
                sug.gameStoryRequirement.storyId = ss[row][13]=="FALSE"?"":ss[row][13];
                if(ss[row][15] == "TRUE") sug.gamePhaseRequirement.phaseId.Add("1");
                if(ss[row][16] == "TRUE") sug.gamePhaseRequirement.phaseId.Add("2");
                if(ss[row][17] == "TRUE") sug.gamePhaseRequirement.phaseId.Add("3");
                if(ss[row][14] == "TRUE") sug.gamePhaseRequirement.phaseId.AddRange(new string[]{"1","2","3"});
                return sug;
            }
    
            List<List<string>> readingCSV() {
                string path = dataConfig.cSVConfigData.pathFile;
                List<List<string>> values= new List<List<string>>();
                string[] readText = File.ReadAllLines(path);
                foreach(string text in readText){
                    List<string> row = new List<string>();
                    foreach (string value in text.Split(",")){
                        row.Add(value);
                    }
                    values.Add(row);
                }

                for (int i=0;i<values.Count;i++)
                {   
                    Console.WriteLine("column : "+i);
                    for(int j=0;j<values[i].Count;j++)
                    {
                        Console.Write("cellll "+j+" : "+values[i][j].ToString()+"; ");

                    }
                    Console.WriteLine("\n-----------------------------------------");
                    
                }
                return values;
            }

            IList<IList<Object>> readingFromGoogleSheets(){
                 string ApplicationName = "Google Sheets API .NET Quickstart";
                string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
                UserCredential credential;
                using (var stream =
                    new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
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
       
            List<SuggestionSheet> feedingnObjectFromSheetsValue(IList<IList<Object>> values){
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
            
            List<SuggestionSheet> feedingnObjectFromCSVValue(List<List<string>> values){
            if (values != null && values.Count > 0)
            {
                suggestionList = new List<SuggestionSheet>(10);
                int rowId=0;
                for (int i=0;i<values.Count;i++)
                {
                    if(values[i][0].ToString() != "Card ID (3001-3999)" && values[i][0].ToString() != "")
                    {
                        SuggestionSheet suggest = new SuggestionSheet();
                        suggest=fillObjectFromCSV(values, i);
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
            
  
            
        }
    }
}