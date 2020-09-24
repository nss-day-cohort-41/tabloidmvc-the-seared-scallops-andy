using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class TagsController : Controller

    {
        private readonly ITagRepository _tagRepository;

        public TagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        // GET: TagsController
        public IActionResult Index()
        {
            var tags = _tagRepository.GetAllTags();
            return View(tags);
        }

        // GET: TagsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TagsController/Create
        public ActionResult Create()
        {

            Tags tag = new Tags();
            return View(tag);
        }

        // POST: TagsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tags tag)
        {
            try
            {
                
                _tagRepository.AddTag(tag);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                
                return View(tag);
            }
        }

        // GET: TagsController/Edit/5
        public ActionResult Edit(int id)
        {
            Tags tag = _tagRepository.GetTagById(id);
            return View(tag);
        }

        // POST: TagsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tags tag)
        {
            try
            {
                _tagRepository.UpdateTag(tag);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }

        // GET: TagsController/Delete/5
        public ActionResult Delete(int id)
        {
            Tags tag = _tagRepository.GetTagById(id);

            return View(tag);
        }

        // POST: TagsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tags tag)
        {
            try
            {
                _tagRepository.DeleteTag(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }
    }
}
