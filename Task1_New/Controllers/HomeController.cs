using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using Task1_New.Migrations;
using Task1_New.Models;
using Task1_New.Models.ViewModel;

namespace Task1_New.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbCont db;

        public HomeController(ILogger<HomeController> logger, DbCont db)
        {
            _logger = logger;
            this.db = db;
        }


        public IActionResult Index()
        {
            StudentParentViewModel sp = new StudentParentViewModel();
            sp.parents = db.Parents.ToList();
            sp.students = db.Students.ToList();
            return View(sp);
        }
        public async Task<IActionResult> Details(int? id)
        {
            var student = db.Students.SingleOrDefault(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }
            var parents = db.Parents.Where(p => p.StudentId == id).ToList();

            // Create the ViewModel
            var viewModel = new School
            {
                ParentsList = parents,
                StudentInfo = student
            };

            // Pass the ViewModel to the view
            return View(viewModel);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var student = db.Students.SingleOrDefault(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }
            var parents = db.Parents.Where(p => p.StudentId == id).ToList();

            // Create the ViewModel
            var viewModel = new School
            {
                ParentsList = parents,
                StudentInfo = student
            };

            // Pass the ViewModel to the view
            return View(viewModel);
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, School viewModel)
        //{

        //    if (id != viewModel.StudentInfo.StudentId)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        db.Students.Update(viewModel.StudentInfo);
        //        //await db.SaveChangesAsync();
        //        // Set StudentId for each Parent
        //        foreach (var parent in viewModel.ParentsList)
        //        {
        //            var prnt = await db.Parents.Where(x => x.Id == parent.Id).FirstOrDefaultAsync();
        //            if (prnt != null)
        //            {
        //                prnt.StudentId = id;
        //                prnt.Name = parent.Name;
        //                prnt.Contact = parent.Contact;
        //                db.Entry(prnt).State = EntityState.Modified;
        //                db.Parents.Update(prnt);
        //            }
        //            else
        //            {
        //                var pare = new ParentInfo()
        //                {
        //                    Name = parent.Name,
        //                    StudentId = viewModel.StudentInfo.StudentId,
        //                    Contact = parent.Contact
        //                };
        //                db.Parents.Add(pare);

        //            }
        //            //db.Parents.Update(pare);
        //        }
        //        await db.SaveChangesAsync();

        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(viewModel);
        //}

        public IActionResult create()
        {
            var viewModel = new School
            {
                StudentInfo = new Student(),
                ParentsList = new List<ParentInfo> { new ParentInfo() } // Initialize with one empty parent
            };

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(School viewModel)
        {

            if (ModelState.IsValid)
            {
                var stud = db.Students.Add(viewModel.StudentInfo);
                await db.SaveChangesAsync();

                // Set StudentId for each Parent
                foreach (var parent in viewModel.ParentsList)
                {
                    if (parent != null)
                    {

                        var pare = new ParentInfo()
                        {
                            Name = parent.Name,
                            StudentId = stud.Entity.StudentId,
                            Contact = parent.Contact
                        };
                        db.Parents.Add(pare);
                    }
                }
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Students == null)
            {
                return NotFound();
            }
            var student = db.Students.SingleOrDefault(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }
            var parents = db.Parents.Where(p => p.StudentId == id).ToList();

            // Create the ViewModel
            var viewModel = new School
            {
                ParentsList = parents,
                StudentInfo = student
            };

            // Pass the ViewModel to the view
            return View(viewModel);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete_Confirm(int? id)
        {
            var data = await db.Students.FindAsync(id);
            if (data != null)
            {
                foreach(var prnt in db.Parents.Where(e=> e.StudentId==id).ToList())
                {
                    db.Parents.Remove(prnt);
                }
                db.Students.Remove(data);
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult AddParent()
        {
            return PartialView("_addParentDetails", new ParentInfo());
        }




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int? id, School viewModel)
        //{

        //    if (id != viewModel.StudentInfo.StudentId)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        db.Students.Update(viewModel.StudentInfo);
        //        // Set StudentId for each Parent
        //        foreach (var parent in viewModel.ParentsList)
        //        {
        //            if (parent != null)
        //            {

        //                var pare = new ParentInfo()
        //                {
        //                    Name = parent.Name,
        //                    StudentId = viewModel.StudentInfo.StudentId,
        //                    Contact = parent.Contact
        //                };
        //                db.Parents.Update(pare);
        //            }
        //        }
        //        await db.SaveChangesAsync();

        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(viewModel);
        //}




        ////Chatgpt 1

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, School viewModel)
        //{
        //    if (id != viewModel.StudentInfo.StudentId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        db.Students.Update(viewModel.StudentInfo);

        //        // Get the current list of parents in the database for this student
        //        var existingParents = await db.Parents.Where(p => p.StudentId == id).ToListAsync();


        //        // Remove parents that are not in the updated list
        //        foreach (var parent in existingParents)
        //        {
        //            if (!viewModel.ParentsList.Any(p => p.Id == parent.Id))
        //            {
        //                db.Parents.Remove(parent);
        //            }
        //        }

        //        // Update or add parents
        //        foreach (var parent in viewModel.ParentsList)
        //        {
        //            var existingParent = existingParents.FirstOrDefault(p => p.Id == parent.Id);

        //            if (existingParent != null)
        //            {
        //                // Update existing parent
        //                existingParent.Name = parent.Name;
        //                existingParent.Contact = parent.Contact;
        //                db.Entry(existingParent).State = EntityState.Modified;
        //            }
        //            else
        //            {
        //                // Add new parent
        //                var newParent = new ParentInfo
        //                {
        //                    Name = parent.Name,
        //                    StudentId = id,
        //                    Contact = parent.Contact
        //                };
        //                db.Parents.Add(newParent);
        //            }
        //        }



        //        await db.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(viewModel);
        //}




        ////Chatgpt2
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, School viewModel)
        //{
        //    if (id != viewModel.StudentInfo.StudentId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Update the student info
        //        db.Students.Update(viewModel.StudentInfo);

        //        // Retrieve all current parents for the student from the database
        //        var existingParents = await db.Parents.Where(p => p.StudentId == id).ToListAsync();

        //        // Create a list to keep track of parent IDs that should not be deleted
        //        var parentIdsToKeep = new HashSet<int>();

        //        // Loop through the parents from the viewModel
        //        foreach (var parent in viewModel.ParentsList)
        //        {
        //            if (parent.Id != null) // Existing parent
        //            {
        //                var existingParent = existingParents.FirstOrDefault(p => p.Id == parent.Id);
        //                if (existingParent != null)
        //                {
        //                    // Update the existing parent's details
        //                    existingParent.Name = parent.Name;
        //                    existingParent.Contact = parent.Contact;
        //                    db.Entry(existingParent).State = EntityState.Modified;

        //                    // Add the parent ID to the list of IDs to keep
        //                    parentIdsToKeep.Add(existingParent.Id);
        //                }
        //            }
        //            else // New parent
        //            {
        //                var newParent = new ParentInfo
        //                {
        //                    Name = parent.Name,
        //                    StudentId = id,
        //                    Contact = parent.Contact
        //                };
        //                db.Parents.Add(newParent);
        //            }
        //        }

        //        // Remove parents that are not in the list of parent IDs to keep
        //        foreach (var parent in existingParents)
        //        {
        //            if (!parentIdsToKeep.Contains(parent.Id))
        //            {
        //                db.Parents.Remove(parent);
        //            }
        //        }

        //        //Save all changes to the database
        //        await db.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(viewModel);
        //}


        //ChatGpt 3

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, School viewModel, string? DeletedParents)
        {
            if (id != viewModel.StudentInfo.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Update student info
                db.Students.Update(viewModel.StudentInfo);

                // Get the current list of parents in the database for this student
                var existingParents = await db.Parents.Where(p => p.StudentId == id).ToListAsync();

                // Remove parents that are in the DeletedParents list
                if (!string.IsNullOrEmpty(DeletedParents))
                {
                    var parentIdsToRemove = DeletedParents.Split(',')
                                                          .Where(id => !string.IsNullOrWhiteSpace(id))
                                                          .Select(int.Parse)
                                                          .ToList();

                    foreach (var parentId in parentIdsToRemove)
                    {
                        var parentToRemove = existingParents.FirstOrDefault(p => p.Id == parentId);
                        if (parentToRemove != null)
                        {
                            db.Parents.Remove(parentToRemove);
                        }
                    }
                }

                // Update or add parents from the form
                foreach (var parent in viewModel.ParentsList)
                {
                    if (parent != null)
                    {
                        var existingParent = existingParents.FirstOrDefault(p => p.Id == parent.Id);

                        if (existingParent != null)
                        {
                            // Update existing parent
                            existingParent.Name = parent.Name;
                            existingParent.Contact = parent.Contact;
                            db.Entry(existingParent).State = EntityState.Modified;
                        }
                        else
                        {
                            // Add new parent
                            var newParent = new ParentInfo
                            {
                                Name = parent.Name,
                                StudentId = id,
                                Contact = parent.Contact
                            };
                            db.Parents.Add(newParent);
                        }
                    }
                }

                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }








        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
