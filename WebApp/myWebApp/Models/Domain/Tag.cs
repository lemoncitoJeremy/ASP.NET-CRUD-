namespace myWebApp.Models.Domain
{
    public class Tag
    {
        public Guid Id  { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        //to connect two models or have an relationship
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
