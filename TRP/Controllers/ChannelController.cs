using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRP.Auth;
using TRP.DTOs;
using TRP.EF;


namespace TRP.Controllers
{
    public class ChannelController : Controller
    {
        PracticeEntities db = new PracticeEntities();
        public static Channel Convert(ChannelDTO dto)
        {
            return new Channel
            {
                ChannelId = dto.ChannelId,
                ChannelName = dto.ChannelName,
                EstablishedYear = dto.EstablishedYear,
                Country = dto.Country
            };
        }

        public static ChannelDTO Convert(Channel entity)
        {
            return new ChannelDTO
            {
                ChannelId = entity.ChannelId,
                ChannelName = entity.ChannelName,
                EstablishedYear = entity.EstablishedYear,
                Country = entity.Country
            };
        }

        public static List<ChannelDTO> Convert(List<Channel> entities)
        {
            return entities.Select(Convert).ToList();
        }

        // View all channels

        public ActionResult List()
        {
            var data = db.Channels.ToList();
            return View(Convert(data));
        }
       


        // Create a new channel (GET)
        [HttpGet]
        public ActionResult Create()
        {
            return View(new ChannelDTO());
        }

        // Create a new channel (POST)
        [HttpPost]
        public ActionResult Create(ChannelDTO dto)
        {
            if (ModelState.IsValid)
            {
                var entity = Convert(dto);
                db.Channels.Add(entity);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(dto);
        }

        // Edit an existing channel (GET)
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = db.Channels.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(Convert(entity));
        }

        // Edit an existing channel (POST)
        [HttpPost]
        public ActionResult Edit(ChannelDTO dto)
        {
            if (ModelState.IsValid)
            {
                var entity = db.Channels.Find(dto.ChannelId);
                if (entity == null)
                {
                    return HttpNotFound();
                }
                db.Entry(entity).CurrentValues.SetValues(dto);
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(dto);
        }

        public ActionResult Details(int id)
        {
            var entity = db.Channels.Find(id);

          
            if (entity == null)
            {
                return HttpNotFound(); // Return a 404 if not found
            }

            // Convert the entity to a DTO and pass it to the view
            var dto = Convert(entity);
            return View(dto);
        }

        [AdminAccess]
        // Delete a channel (GET)
        [HttpGet]
       
        public ActionResult Delete(int id)
        {
            var data = db.Channels.Find(id);
            if (data == null)
            {
                return HttpNotFound(); // Return a 404 error if the channel is not found
            }

            return View(Convert(data)); // Ensure Convert handles null safely
        }

        [AdminAccess]
        [HttpPost]
        public ActionResult Delete(int id, string dcsn)
        {
            if (dcsn.Equals("Yes", StringComparison.OrdinalIgnoreCase)) // Ensure case-insensitive comparison
            {
                var data = db.Channels.Find(id);
                if (data != null)
                {
                    db.Channels.Remove(data); // Safely handle null channels
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }





    }

}