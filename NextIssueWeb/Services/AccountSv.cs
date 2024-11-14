using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static NextIssueWeb.Models.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NextIssueWeb.Services
{
    public class AccountSv : Controller
    {
        public NextIssueContext _db;
        public LoggerSv _logger;
        public string _config;
        private string encrypword = "NextIssueSystemForAuthenticationKeyAndSendBackToMe";
        readonly string Services = "AccountSv";

        public AccountSv(NextIssueContext context)
        {
            _db = context;
        }
        public AccountSv(string config)
        {
            _config = config;
        }

        #region Get
        public ResponseModel<Nuser> Login(string user, string password)
        {
            var rs = new ResponseModel<Nuser>();
            try
            {
                var target = _db.Nusers.Where(db => db.Username == user || db.Aka == user).FirstOrDefault();
                if (target != null)
                {
                    string pwAfter = Encoding.UTF8.GetBytes(password).ToString();
                    if (pwAfter == Encoding.UTF8.GetBytes(target.Password).ToString()) // รหัสผ่านถูก
                    {
                        rs.Data = target;
                        rs.IsSuccess = true;
                        rs.Message = "200 : Login Successfully";
                        rs.Code = 200;
                    }
                }
                else
                {
                    rs.IsSuccess = false;
                    rs.Message = "400 : NOT FOUND USER";
                    rs.Code = 400;
                }
            }
            catch (SqlException ex)
            {
                rs.IsSuccess = false;
                rs.Message = "53 :" + ex.Message;
                rs.Code = 53;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = "500 :" + ex.Message;
                rs.Code = 500;
            }
            return rs;
        }
        public ResponseModel<List<Nuser>> GetAllUser()
        {
            var rs = new ResponseModel<List<Nuser>>();
            try
            {
                rs.Data = _db.Nusers.ToList();
                rs.Code = 200;
                rs.Message = "Get List Of User Successfully";
                rs.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Code = 500;
                rs.Message = ex.Message;
            }
            return rs;
        }
        public ResponseModel<Nposition> GetPositionById(int id)
        {
            var returnThis = new ResponseModel<Nposition>();
            try
            {
                returnThis.Data = _db.Npositions.Where(x => x.Id == id).FirstOrDefault();
                returnThis.Code = 200;
                returnThis.Message = "Get position successfully";
            }
            catch (Exception ex)
            {
                returnThis.Data = null;
                returnThis.Code = 500;
                returnThis.Message = "Get position fail :" + ex.Message;
            }
            return returnThis;
        }
        public ResponseModel<List<Nposition>> GetAllPosition()
        {
            var returnThis = new ResponseModel<List<Nposition>>();
            try
            {
                returnThis.Data = _db.Npositions.ToList();
                returnThis.Code = 200;
                returnThis.Message = "Get position successfully";
            }
            catch (Exception ex)
            {
                returnThis.Data = null;
                returnThis.Code = 500;
                returnThis.Message = "Get position fail :" + ex.Message;
            }
            return returnThis;
        }
        public ResponseModel<List<Nposition>> GetListsPositionByGroupId(int groupId)
        {
            var returnThis = new ResponseModel<List<Nposition>>();
            try
            {
                returnThis.Data = _db.Npositions.ToList();
                returnThis.Code = 200;
                returnThis.Message = "Get position successfully";
            }
            catch (Exception ex)
            {
                returnThis.Data = null;
                returnThis.Code = 500;
                returnThis.Message = "Get position fail :" + ex.Message;
            }
            return returnThis;
        }

        public ResponseModel<Nuser> GetUserById(int id)
        {
            ResponseModel<Nuser> rs = new ResponseModel<Nuser>();
            try
            {
                var data = _db.Nusers.Where(db => db.Id == id).FirstOrDefault();
                if(data != null)
                {
                    data.Password = null;
                }
                rs.Data = data;
                rs.Code = 200;
                rs.Message = "Get User Successfully";
                rs.IsSuccess = true;
            }
            catch (Exception e)
            {
                rs.Code = 500;
                rs.Message = "Cannot User Successfully";
                rs.IsSuccess = true;
            }
            return rs;
        }
        public ResponseModel<List<Nuser>> GetListUserByGroupId(int id)
        {
            ResponseModel<List<Nuser>> rs = new ResponseModel<List<Nuser>>();
            try
            {
                var data = _db.Nusers
                  .Where(db => db.PositionId == id)
                  .ToList();

                // ตั้งค่า Password เป็น null สำหรับทุกรายการในลิสต์
                data.ForEach(user => user.Password = null);

                rs.Data = data;
                rs.Code = 200;
                rs.Message = "Get User Successfully";
                rs.IsSuccess = true;
            }
            catch (Exception e)
            {
                rs.Code = 500;
                rs.Message = "Cannot User Successfully";
                rs.IsSuccess = true;
            }
            return rs;
        }

        public ResponseModel<Nuser> GetUserByName(string username)
        {
            ResponseModel<Nuser> rs = new ResponseModel<Nuser>();
            try
            {
                var data = _db.Nusers.ToList().Where(db => db.Username == username).FirstOrDefault();
                data.Password = null;
                rs.Data = data;
                rs.Code = 200;
                rs.Message = "Get User Successfully";
                rs.IsSuccess = true;
            }
            catch (Exception e)
            {
                rs.Code = 500;
                rs.Message = "Cannot User Successfully";
                rs.IsSuccess = true;
            }
            return rs;
        }
        public ResponseModel<List<Nuser>> GetUserListsByGroupId(int id)
        {
            ResponseModel<List<Nuser>> rs = new ResponseModel<List<Nuser>>();
            try
            {
                rs.Data = _db.Nusers.ToList().Where(db => db.Id == id).ToList();
                rs.Code = 200;
                rs.Message = "Get User Lists Successfully";
                rs.IsSuccess = true;
            }
            catch (Exception e)
            {
                rs.Code = 500;
                rs.Message = "Cannot User Successfully";
                rs.IsSuccess = true;
            }
            return rs;
        }
        #endregion

        #region Insert
        public ResponseModel<Nuser> InsertUser(Nuser data)
        {
            var rs = new ResponseModel<Nuser>();
            try
            {
               _db.Nusers.Add(data);
                _db.SaveChanges();
                rs.Data = data;
                rs.Code = 200;
                rs.Message = "Insert User Successfully";
                rs.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Code = 500;
                rs.Message = ex.Message;
            }
            return rs;
        }
        public ResponseModel<List<Nuser>> InsertUserLists(List<Nuser> data)
        {
            var rs = new ResponseModel<List<Nuser>>();
            try
            {
                _db.AddRange(data);
                rs.Data = data;
                rs.Code = 200;
                rs.Message = "Insert User Successfully";
                rs.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Code = 500;
                rs.Message = ex.Message;
            }
            return rs;
        }

        #endregion

        #region Update

        public ResponseModel<Nuser> UpdateUser(ViewModel data)
        {
            var rs = new ResponseModel<Nuser>();
            try
            {
                Nuser user = GetUserById(data.Nuser.Id).Data;
                user.UpdateDate = DateTime.Now;
                user.UpdateBy = data.userId;
                user.Username = (data.Nuser.Username != null && data.Nuser.Username != user.Username) ? data.Nuser.Username : user.Username;
                user.Aka = (data.Nuser.Aka != null && data.Nuser.Aka != user.Aka) ? data.Nuser.Aka : user.Aka;

                _db.Nusers.Update(user);
                _db.SaveChanges();

                rs.Data = GetUserById(user.Id).Data;
                rs.Code = 200;
                rs.Message = "Update User Successfully";
                rs.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Code = 500;
                rs.Message = ex.Message;
            }
            return rs;
        }
        public ResponseModel<List<Nuser>> UpdateUserLists(List<Nuser> data)
        {
            var rs = new ResponseModel<List<Nuser>>();
            try
            {
                
                _db.UpdateRange(data);
                _db.SaveChanges();

                rs.Data = data;
                rs.Code = 200;
                rs.Message = "Update User Successfully";
                rs.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Code = 500;
                rs.Message = ex.Message;
            }
            return rs;
        }
        public ResponseModel<Nuser> ChangPassword(Nuser data)
        {
            var rs = new ResponseModel<Nuser>();
            try
            {
                Nuser user = GetUserById(data.Id).Data;
                user.UpdateDate = DateTime.Now;
                //UpdateBy = data.UserId
                user.Username = (data.Username != null && data.Username != user.Username) ? data.Username : user.Username;
                user.Password = (data.Password != null
                    && Encoding.UTF8.GetBytes(data.Password).ToString()
                    != Encoding.UTF8.GetBytes(user.Password).ToString())
                    ? Encoding.UTF8.GetBytes(data.Password).ToString()
                    : Encoding.UTF8.GetBytes(user.Password).ToString();
                _db.Nusers.Update(user);
                _db.SaveChanges();

                rs.Data = GetUserById(user.Id).Data;
                rs.Code = 200;
                rs.Message = "Update User Successfully";
                rs.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Code = 500;
                rs.Message = ex.Message;
            }
            return rs;
        }

        #endregion

        #region Delete
        public ResponseModel<Nuser> DeleteUserById(int id)
        {
            ResponseModel<Nuser> rs = new ResponseModel<Nuser>();
            var user = GetUserById(id).Data;
            Nlogger nlogger = new Nlogger();
            try
            {
                _db.Nusers.Remove(user);
                rs.Code = 200;
                rs.Message = "Delete User Successfully";
                rs.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rs.Code = 500;
                rs.Message = ex.Message;
                rs.IsSuccess = false;
            }
            return rs;
        }
        public ResponseModel<List<Nuser>> DeleteUserLists(List<Nuser> data)
        {
            var rs = new ResponseModel<List<Nuser>>();
            try
            {
                var userlst = new List<Nuser>();
                foreach (var record in data)
                {
                    Nuser user = GetUserById(record.Id).Data;
                    userlst.Add(user);
                }
                _db.RemoveRange(userlst);
                rs.Code = 200;
                rs.Message = "Delete User Successfully";
                rs.IsSuccess = true;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Code = 500;
                rs.Message = ex.Message;
            }
            return rs;
        }

        #endregion

        #region GenerateToken
        public string GenerateToken(string userId, string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("NextIssueSystemForAuthenticationKeyAndSendBackToMe"); // ใช้คีย์ลับสำหรับเซ็นชื่อ JWT

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("id", userId),
                new Claim(ClaimTypes.Name, username)
            }),
                Expires = DateTime.UtcNow.AddMinutes(20), // ตั้งเวลาให้ Token หมดอายุ
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion
    }
}
