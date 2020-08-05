using System.Collections.Generic;
using System.Drawing;

namespace TextAnalyzer.Modules.Models
{
    public class CompareByStartIndex : IComparer<EntryModel>
    {
        public int Compare(EntryModel o1, EntryModel o2)
        {
            return o1.StartIndex.CompareTo(o2.StartIndex);
        }
    }

    public class CompareByEndIndex : IComparer<EntryModel>
    {
        public int Compare(EntryModel o1, EntryModel o2)
        {
            return o1.EndIndex.CompareTo(o2.EndIndex);
        }
    }
    public class EntryModel
    {
        public static int Offset { get; set; }
        private int startIndex;
        private int endIndex;
        public Color TextColor { get; set; }
        public int StartIndex
        {
            get { return startIndex + Offset; }
            set { startIndex = value; }
        }
        public int EndIndex
        {
            get { return endIndex + Offset; }
            set { endIndex = value; }
        }
        public int Length
        {
            get { return EndIndex - StartIndex; }
        }
    }

    public class EntryModelNum : EntryModel
    {
        public long Num { get; set; }
    }
}
