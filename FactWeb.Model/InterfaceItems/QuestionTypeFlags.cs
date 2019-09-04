using System;

namespace FactWeb.Model.InterfaceItems
{
    public class QuestionTypeFlags
    {
        [Flags]
        public enum QuestionType
        {
            None = 0,
            TextArea = 1,
            RadioButtons = 2,
            Checkboxes = 4,
            DocumentUpload = 8,
            Date = 16,
            DateRange = 32,
            PeoplePicker = 64,
            TextBox = 128
        }

        public bool TextArea { get; set; }
        public bool RadioButtons { get; set; }
        public bool Checkboxes { get; set; }
        public bool DocumentUpload { get; set; }
        public bool Date { get; set; }
        public bool DateRange { get; set; }
        public bool PeoplePicker { get; set; }
        public bool TextBox { get; set; }

        public byte AllTypes
        {
            get
            {
                byte value = 0;
                if (this.TextArea) value |= (byte) QuestionType.TextArea;
                if (this.RadioButtons) value |= (byte) QuestionType.RadioButtons;
                if (this.Checkboxes) value |= (byte) QuestionType.Checkboxes;
                if (this.DocumentUpload) value |= (byte) QuestionType.DocumentUpload;
                if (this.Date) value |= (byte) QuestionType.Date;
                if (this.DateRange) value |= (byte) QuestionType.DateRange;
                if (this.PeoplePicker) value |= (byte) QuestionType.PeoplePicker;
                if (this.TextBox) value |= (byte) QuestionType.TextBox;

                return value;
            }
        }

        public static QuestionTypeFlags Set(byte value)
        {
            var instance = new QuestionTypeFlags
            {
                TextArea = ((value & (byte)QuestionType.TextArea) != 0),
                RadioButtons = ((value & (byte)QuestionType.RadioButtons) != 0),
                Checkboxes = ((value & (byte)QuestionType.Checkboxes) != 0),
                DocumentUpload = ((value & (byte)QuestionType.DocumentUpload) != 0),
                Date = ((value & (byte)QuestionType.Date) != 0),
                DateRange = ((value & (byte)QuestionType.DateRange) != 0),
                PeoplePicker = ((value & (byte)QuestionType.PeoplePicker) != 0),
                TextBox = ((value & (byte)QuestionType.TextBox) != 0)
            };

            return instance;
        }
    }
}
