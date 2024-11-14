using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace NextIssueWeb.Services
{
    public class TicketSv : Controller
    {
        private readonly NextIssueContext _db;
        private readonly string encrypword = "NextIssueSystemForAuthenticationKeyAndSendBackToMe";
        private readonly string Services = "LoggerSv";

        public TicketSv(NextIssueContext context)
        {
            _db = context;
        }

        // CREATE
        public ResponseModel<Nticket> CreateTicket(Nticket model)
        {
            var response = new ResponseModel<Nticket>();
            try
            {
                
                model.CreateDate = DateTime.Now;
                model.UpdateDate = DateTime.Now;
                _db.Ntickets.Add(model);
                _db.SaveChanges();
                response.IsSuccess = true;
                response.Data = model;
                response.Message = "Ticket created successfully";
                response.Code = 200;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Code = 500;
            }
            return response;
        }

        // READ
        public ResponseModel<Nticket> GetTicketById(int id)
        {
            var response = new ResponseModel<Nticket>();
            try
            {
                var ticket = _db.Ntickets.Where(x => x.Id == id).Include(p => p.Project).FirstOrDefault();
                if (ticket != null)
                {
                    response.IsSuccess = true;
                    response.Data = ticket;
                    response.Message = "Ticket retrieved successfully";
                    response.Code = 200;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Ticket not found";
                    response.Code = 404;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Code = 500;
            }
            return response;
        }

        public ResponseModel<Npicture> GetPictureOfTicketId(int id)
        {
            var response = new ResponseModel<Npicture>();
            try
            {
                var ticket = _db.Npictures.Where(t => t.Id == id).FirstOrDefault();
                if (ticket != null)
                {
                    response.IsSuccess = true;
                    response.Data = ticket;
                    response.Message = "Ticket retrieved successfully";
                    response.Code = 200;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Ticket not found";
                    response.Code = 404;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Code = 500;
            }
            return response;
        }

        public ResponseModel<List<Nticket>> GetTicketLists()
        {
            var response = new ResponseModel<List<Nticket>>();
            try
            {
                var ticket = _db.Ntickets.
                    Include(p=>p.Project)
                    .Include(x=>x.Informer)
                    .Include(r=>r.Responsible)
                    .Include(rg=>rg.ResponsibleGroup)
                    .Include(st=>st.Status)
                    .ToList();

                foreach (var item in ticket)
                {
                    
                    if(item.Informer == null)
                    {
                        var user = new Nuser()
                        {
                            Id = 0,
                            Aka = "ไม่ระบุผู้รับผิดชอบ",
                            Username = "ไม่ระบุผู้รับผิดชอบ",
                            Password = null
                        };
                        item.Informer = user;
                    }
                    if(item.Responsible == null){
                        var user = new Nuser()
                        {
                            Id = 0,
                            Aka = "ไม่ระบุผู้รับผิดชอบ",
                            Username = "ไม่ระบุผู้รับผิดชอบ",
                            Password = null
                        };
                        item.Responsible = user;
                    }
                    item.Informer.Password = null;
                    item.Responsible.Password = null;
                }
                if (ticket != null)
                {
                    response.IsSuccess = true;
                    response.Data = ticket;
                    response.Message = "Ticket retrieved successfully";
                    response.Code = 200;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Ticket not found";
                    response.Code = 404;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Code = 500;
            }
            return response;
        }

        public ResponseModel<List<Nticket>> GetTicketListsByProjectId(Guid projectId)
        {
            var response = new ResponseModel<List<Nticket>>();
            try
            {
                var ticket = _db.Ntickets.Where(db=>db.ProjectId == projectId).ToList();
                if (ticket != null)
                {
                    response.IsSuccess = true;
                    response.Data = ticket;
                    response.Message = "Ticket retrieved successfully";
                    response.Code = 200;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Ticket not found";
                    response.Code = 404;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Code = 500;
            }
            return response;
        }

        // UPDATE
        public ResponseModel<Nticket> UpdateTicket(Nticket model)
        {
            var response = new ResponseModel<Nticket>();
            try
            {
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                response.IsSuccess = true;
                response.Data = model;
                response.Message = "Update Ticket Successfully";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Code = 500;
            }
            return response;
        }

        // DELETE
        public ResponseModel<bool> DeleteTicket(int id)
        {
            var response = new ResponseModel<bool>();
            try
            {
                var ticket = _db.Ntickets.FirstOrDefault(t => t.Id == id);
                if (ticket != null)
                {
                    _db.Ntickets.Remove(ticket);
                    _db.SaveChanges();

                    response.IsSuccess = true;
                    response.Data = true;
                    response.Message = "Ticket deleted successfully";
                    response.Code = 200;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Data = false;
                    response.Message = "Ticket not found";
                    response.Code = 404;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = false;
                response.Message = ex.Message;
                response.Code = 500;
            }
            return response;
        }
    }
}
