namespace TaskNetic.Client.DTO
{
    public class NewBoardLabel
    {
        public string LabelName { get; set; }
        public string ColorCode { get; set; }
        public string Comment { get; set; }

        public NewBoardLabel(string labelName, string colorCode, string comment)
        {
            LabelName = labelName;
            ColorCode = colorCode;
            Comment = comment;
        }
    }
}
