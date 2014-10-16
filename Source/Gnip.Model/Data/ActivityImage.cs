namespace Krowiorsch.Gnip.Model.Data
{
    /// <summary>
    /// Represents a graphical image. Objects of this type MAY contain an additional fullImage property whose value is an Activity Streams 
    /// Media Link to a "full-sized" representation of the image.
    /// </summary>
    public class ActivityImage : Activity
    {
        /// <summary> texttuell content of the Image </summary>
        public string ImageContent { get; set; }

        /// <summary> Link to HtmlView </summary>
        public string HtmlViewLink { get; set; }

        /// <summary> Link to a Preview </summary>
        public string PreviewLink { get; set; }

        /// <summary> EnclosureLink </summary>
        public string EnclosureLink { get; set; }

        /// <summary> category of the image </summary>
        public string[] Category { get; set; }

        /// <summary> an Activity Streams Media Link to a "full-sized" representation of the image </summary>
        public string FullImage { get; set; }


        public override string GetContent()
        {
            return ImageContent;
        }
    }
}