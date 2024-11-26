using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRP.DTOs;
using TRP.EF;
using System.Data.Entity; // Required for Include method in EF 6.x




namespace TRP.Controllers
{
    public class ProgramController : Controller
    {
        // GET: Program
        PracticeEntities db = new PracticeEntities();

        // Convert ProgramDTO to Program model
        public static Program Convert(ProgramDTO dto)
        {
            return new Program
            {
                ProgramId = dto.ProgramId,
                ProgramName = dto.ProgramName,
                TRPScore = dto.TRPScore,
                ChannelId = dto.ChannelId,
                AirTime = dto.AirTime
            };
        }

        // Convert Program model to ProgramDTO
        public static ProgramDTO Convert(Program entity)
        {
            return new ProgramDTO
            {
                ProgramId = entity.ProgramId,
                ProgramName = entity.ProgramName,
                TRPScore = entity.TRPScore,
                ChannelId = entity.ChannelId,
                AirTime = entity.AirTime
            };
        }

        public static List<ProgramDTO> Convert(List<Program> entities)
        {
            return entities.Select(Convert).ToList();
        }

        // View all programs, grouped by channel
        public ActionResult List()
        {
            var programs = db.Programs.Include(p => p.Channel).ToList();

            var groupedPrograms = programs
                .GroupBy(p => p.Channel) // Group by Channel
                .Select(g => new ProgramGroupDTO
                {
                    ChannelName = g.Key.ChannelName,
                    Programs = g.Select(p => new ProgramDTO
                    {
                        ProgramId = p.ProgramId,
                        ProgramName = p.ProgramName,
                        TRPScore = p.TRPScore,
                        AirTime = p.AirTime,
                        ChannelId = p.ChannelId
                    }).ToList()
                }).ToList();

            return View(groupedPrograms);
        }


        // Create a new program (GET)
        // Create a new program (GET)
        [HttpGet]
        public ActionResult Create()
        {
            var channels = db.Channels.Select(c => new SelectListItem
            {
                Value = c.ChannelId.ToString(),
                Text = c.ChannelName
            }).ToList();

            var dto = new ProgramDTO
            {
                Channels = channels
            };
            return View(dto);
        }


        // Create a new program (POST)
        [HttpPost]
        public ActionResult Create(ProgramDTO dto)
        {
            if (ModelState.IsValid)
            {
                // Check if the program name is unique within the channel
                var existingProgram = db.Programs.FirstOrDefault(p => p.ProgramName == dto.ProgramName && p.ChannelId == dto.ChannelId);
                if (existingProgram != null)
                {
                    ModelState.AddModelError("ProgramName", "Program name must be unique within the channel.");
                    dto.Channels = db.Channels.Select(c => new SelectListItem
                    {
                        Value = c.ChannelId.ToString(),
                        Text = c.ChannelName
                    }).ToList();
                    return View(dto);
                }

                // Convert DTO to Program entity
                var entity = Convert(dto);
                db.Programs.Add(entity);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            // If validation fails, populate the channels dropdown again
            dto.Channels = db.Channels.Select(c => new SelectListItem
            {
                Value = c.ChannelId.ToString(),
                Text = c.ChannelName
            }).ToList();
            return View(dto);
        }

        // Edit an existing program (GET)
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = db.Programs.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }

            var dto = Convert(entity);
            dto.Channels = db.Channels.Select(c => new SelectListItem
            {
                Value = c.ChannelId.ToString(),
                Text = c.ChannelName
            }).ToList();
            return View(dto);
        }

        // Edit an existing program (POST)
        [HttpPost]
        public ActionResult Edit(ProgramDTO dto)
        {
            if (ModelState.IsValid)
            {
                var entity = db.Programs.Find(dto.ProgramId);
                if (entity == null)
                {
                    return HttpNotFound();
                }

                // Check if the program name is unique within the channel
                var existingProgram = db.Programs.FirstOrDefault(p => p.ProgramName == dto.ProgramName && p.ChannelId == dto.ChannelId && p.ProgramId != dto.ProgramId);
                if (existingProgram != null)
                {
                    ModelState.AddModelError("ProgramName", "Program name must be unique within the channel.");
                    dto.Channels = db.Channels.Select(c => new SelectListItem
                    {
                        Value = c.ChannelId.ToString(),
                        Text = c.ChannelName
                    }).ToList();
                    return View(dto);
                }

                db.Entry(entity).CurrentValues.SetValues(dto);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            dto.Channels = db.Channels.Select(c => new SelectListItem
            {
                Value = c.ChannelId.ToString(),
                Text = c.ChannelName
            }).ToList();
            return View(dto);
        }

        // Delete a program (GET)
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var entity = db.Programs.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(Convert(entity));
        }

        // Delete a program (POST)
        [HttpPost]
        public ActionResult Delete(int Id, string dcsn)
        {
            if (dcsn.Equals("Yes"))
            {
                var entity = db.Programs.Find(Id);
                if (entity != null)
                {
                    db.Programs.Remove(entity);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
    }
}