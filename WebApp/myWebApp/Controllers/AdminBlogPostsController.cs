using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myWebApp.Models.Domain;
using myWebApp.Models.ViewModels;
using myWebApp.Repositories;

namespace myWebApp.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository , IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //get tags from repository
            var tags = await tagRepository.GetAllAsync();
            var tagsModel = new AddBlogPostRequest
            {
                DisplayTags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            return View(tagsModel);
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            //Map view model to domain model before we can actually pass it
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,

            };
            // Map Tags From Selected Tags
            // loop through the id's and get the values from the database
                                           //These are the tags coming in
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags) 
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
               var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);
                if (existingTag != null) 
                {
                    selectedTags.Add(existingTag);
                    
                }
            }
            //Mapping Tags back to domain model
            blogPost.Tags = selectedTags;

            await blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("List");
        }



        [HttpGet]
        public async Task<IActionResult> List() 
        {
            // Call The Repository
            var blogPosts= await blogPostRepository.GetAllAsync();

            return View(blogPosts);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //Retrieve the result from the repository
            var blogpost = await blogPostRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllAsync();

            if (blogpost != null) 
            {
                var model = new EditBlogPostRequest
                {
                    Id = blogpost.Id,
                    Heading = blogpost.Heading,
                    PageTitle = blogpost.PageTitle,
                    Content = blogpost.Content,
                    Author = blogpost.Author,
                    FeaturedImageUrl = blogpost.FeaturedImageUrl,
                    UrlHandle = blogpost.UrlHandle,
                    ShortDescription = blogpost.ShortDescription,
                    PublishedDate = blogpost.PublishedDate,
                    Visible = blogpost.Visible,
                    DisplayTags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    }),
                    SelectedTags = blogpost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }
            //if value is null
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest) 
        {
            //map view model back to domain model
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                PublishedDate = editBlogPostRequest.PublishedDate,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible,
            };

            //Map Tags into Domain Model
            var selectedTags = new List<Tag>();
                                    //This is the already selected tags
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);

                    if (foundTag != null) 
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }
            blogPostDomainModel.Tags = selectedTags;


            //Submit information to repository to update

            var updatedBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);
            if (updatedBlog != null) 
            {
                //Show success notification
                return RedirectToAction("List");
            }

            //Show Error Notification
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            //Talk to the repository to delete this blogpost and tags
           var deletedBlogPost =  await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);
            if(deletedBlogPost != null) 
            {
                //Show Success Notification
                return RedirectToAction("List");
            }
            //Show Error Notification
            return RedirectToAction("Edit", new { id = editBlogPostRequest.Id });
        }
    }
}
