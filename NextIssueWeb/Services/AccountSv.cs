using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using NextIssueWeb.Data;
using NextIssueWeb.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static NextIssueWeb.Models.Metadata;
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
        public ResponseModel<List<Metadata.NuserWithPermission>> GetAllUserPermission()
        {
            var rs = new ResponseModel<List<Metadata.NuserWithPermission>>();
            try
            {
                var userLists = _db.Nusers.ToList();
                var userWithpermission = new List<Metadata.NuserWithPermission>();
                foreach (var user in userLists)
                {
                    var record = new Metadata.NuserWithPermission()
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Aka = user.Aka,
                        Permission = GetPositionById(user.Id).Data.Name,
                    };
                    userWithpermission.Add(record);
                }
                rs.Data = userWithpermission;
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
                var merge = _db.MergeuserPositions.Where(db => db.UserId == id).FirstOrDefault();
                returnThis.Data = _db.Npositions.Where(db => db.Id == merge.Id).FirstOrDefault();
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
                rs.Data = _db.Nusers.ToList().Where(db => db.Id == id).FirstOrDefault();
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
        public ResponseModel<List<Nposition>> GetPemissionUserById(int id)
        {
            ResponseModel<List<Nposition>> rs = new ResponseModel<List<Nposition>>();
            try
            {
                var UxP = _db.MergeuserPositions.Where(db=>db.UserId == id).OrderBy(db=>db.Id).ToList();
                rs.Data = _db.Npositions.ToList().Where(db => db.Id == UxP.First().PositionId).ToList();
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

        #endregion

        #region Insert
        public ResponseModel<Nuser> InsertUser(Metadata.NuserCreate data)
        {
            var rs = new ResponseModel<Nuser>();
            try
            {
                Nuser user = new Nuser()
                {
                    Username = data.Username,
                    Password = Encoding.UTF8.GetBytes(data.Password).ToString(),
                    Aka = data.Aka,
                    //CreateBy = data.UserId
                    //UpdateBy = data.UserId

                    CreateDate = DateTime.Now,

                    UpdateDate = DateTime.Now
                };
                rs.Data = GetUserById(user.Id).Data;
                _db.SaveChanges();

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
        public ResponseModel<List<Nuser>> InsertUserLists(List<Metadata.NuserCreate> data)
        {
            var rs = new ResponseModel<List<Nuser>>();
            try
            {
                var userlst = new List<Nuser>();
                foreach (var record in data)
                {
                    Nuser user = new Nuser()
                    {
                        Username = record.Username,
                        Password = Encoding.UTF8.GetBytes(record.Password).ToString(),
                        Aka = record.Aka,
                        //Create = record.UserId,
                        //UpdateBy = record.UserId,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    userlst.Add(user);
                }
                _db.AddRange(userlst);
                rs.Data = userlst;
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

        public ResponseModel<Nuser> UpdateUser(Metadata.NuserUpdate data)
        {
            var rs = new ResponseModel<Nuser>();
            try
            {
                Nuser user = GetUserById(data.Id).Data;
                user.UpdateDate = DateTime.Now;
                //UpdateBy = record.UserId,
                user.Username = (data.Username != null && data.Username != user.Username) ? data.Username : user.Username;
                user.Aka = (data.Aka != null && data.Aka != user.Aka) ? data.Aka : user.Aka;

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
        public ResponseModel<List<Nuser>> UpdateUserLists(List<Metadata.NuserUpdate> data)
        {
            var rs = new ResponseModel<List<Nuser>>();
            try
            {
                var userlst = new List<Nuser>();
                foreach (var record in data)
                {
                    Nuser user = GetUserById(record.Id).Data;
                    user.UpdateDate = DateTime.Now;
                    //UpdateBy = data.UserId
                    user.Username = (record.Username != null && record.Username != user.Username) ? record.Username : user.Username;
                    user.Aka = (record.Aka != null && record.Aka != user.Aka) ? record.Aka : user.Aka;
                    userlst.Add(user);
                }
                _db.UpdateRange(userlst);
                _db.SaveChanges();

                rs.Data = userlst;
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
        public ResponseModel<Nuser> ChangPassword(Metadata.NuserChangPassword data)
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
        public ResponseModel<List<Nuser>> DeleteUserLists(List<Metadata.NuserUpdate> data)
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
