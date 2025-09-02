using System;
using System.IO;
using System.Threading.Tasks;

public class Categorizer : Agent
{
    protected string pathToResult = string.Empty;

    public Categorizer(string name, string modelName, string convoContext = "", string pathToResult = "",  string loggerPath = "") : base(name, modelName, convoContext, loggerPath)
    {
        if (!string.IsNullOrEmpty(pathToResult)) { 
            this.pathToResult = String.Concat(pathToResult, name, "-", DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"), ".json");
            File.AppendAllText(this.pathToResult, string.Empty);
        }
    }

    public async Task CategorizeMessage (string message, bool doLog = true) {
        string asw = string.Empty;
            try{
                await foreach (var answerToken in chat.SendAsync(message)) {asw += answerToken;}
                asw = asw.Replace("`", "").Replace("json", "").Trim();
                if (doLog&&logger!=null) {logger.Log(name+" [AGENT]: "+asw);}  
                    if (!string.IsNullOrEmpty(pathToResult)) {
                        if(!string.IsNullOrEmpty(File.ReadAllText(pathToResult)))
                        {
                            var existingJson = Utils.OpenJsonFile(pathToResult);
                            var answerJson = Utils.ConvertTextToJson(asw);
                            _ = Utils.MergeJsonWithAverages(pathToResult, answerJson);
                           
                        } else {
                            Console.WriteLine("DEBUG4");
                            var answerJson = Utils.ConvertTextToJson(asw);
                            _ = Utils.WriteJsonToFile(pathToResult, answerJson);
                        }
                    }
                } catch(Exception e) {
                    Console.WriteLine(e);
            }

       

    }

}
