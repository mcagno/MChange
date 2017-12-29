namespace MagicPurse.Library
{
    public class MagicPurseBase
    {
        protected readonly long[] Coins =
        {
            1, //Halfpenny 0.5d
            2, //Penny 1d
            6, //Threepence 3d
            12, //Sixpence 6d
            24, //Shilling 1/-
            48, //Two shillings 
            60 //Two shillings and sixpence
        };

        protected ISplitter Splitter;
    }
}