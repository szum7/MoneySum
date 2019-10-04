namespace ExcelWorkerApp
{
    public class MyObj
    {
        public int id { get; set; }
        public string title { get; set; }
        public override string ToString()
        {
            return $"{id}-{title}";
        }
    }
}
