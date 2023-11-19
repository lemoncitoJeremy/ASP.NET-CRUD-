using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myWebApp.Data;
using myWebApp.Models.Domain;
using myWebApp.Models.ViewModels;
using myWebApp.Repositories;

namespace myWebApp.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }



        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };

            await tagRepository.AddAsync(tag);
            return RedirectToAction("List");
        }



        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List() {

            // use dbContext to read the tags
            var tags = await tagRepository.GetAllAsync();

            return View(tags);
        }

        
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagRepository.GetAsync(id);
            
            if (tag != null) 
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };
                return View(editTagRequest);
                //return to the view with the editTagRequest value
            }
            
            return View(null);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            };
            var updated = await tagRepository.UpdateAsync(tag);

            if (updated != null)
            {
                //SHOW SUCCESS NOTIFICATION
            }
            else 
            {
                //show error notification
            }


            //return RedirectToAction("Edit" , new { id = editTagRequest.Id});
            return RedirectToAction("List");
        }



        [HttpPost]
        public async Task <IActionResult> Delete(EditTagRequest editTagRequest) 
        {
            var deletedTag =  await tagRepository.DeleteAsync(editTagRequest.Id);

            if (deletedTag != null)
            {
                //SHOW SUCCESS NOTIFICATION
                return RedirectToAction("List");
            }
           
                //show an error notification  //if null we return the value back to the edit page
            return RedirectToAction("Edit", new {id = editTagRequest.Id});

        }



    } 
}
