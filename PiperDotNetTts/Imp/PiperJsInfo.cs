using Newtonsoft.Json;

namespace DotNetTtsDeepSpeech.Imp
{
    public class PiperJsInfo
    {
        [JsonProperty("dataset")]
        public string Dataset { get; set; }
            
        [JsonProperty("audio")]
        public PAudioProperty Audio { get; set; }
            
        [JsonProperty("espeak")]
        public PropertyEspeak Espeak { get; set; }
            
        [JsonProperty("language")]
        public PropertyLanguage Language { get; set; }
    }
    
    public class PAudioProperty
    {
        [JsonProperty("sample_rate")]
        public int SampleRate{get;set;}
            
        [JsonProperty("quality")]
        public string Quality{get;set;}
    }
    public class PropertyEspeak
    {
        [JsonProperty("voice")]
        public string Voice{get;set;}
    }
    public class PropertyLanguage
    {
        [JsonProperty("code")]
        public string Code{get;set;}
            
        [JsonProperty("family")]
        public string Family{get;set;}
            
        [JsonProperty("region")]
        public string Region{get;set;}
            
        [JsonProperty("name_native")]
        public string NameNative{get;set;}
            
        [JsonProperty("name_english")]
        public string NameEnglish{get;set;}
            
        [JsonProperty("country_english")]
        public string CountryEnglish{get;set;}
    }
}