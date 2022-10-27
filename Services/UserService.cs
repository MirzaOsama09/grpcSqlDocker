using Grpc.Core;
using GrpcGreeter.Models;
using GrpcGreeter.Protos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace GrpcGreeter.Services
{
    public class UserService : Protos.User.UserBase
    {
        private readonly ILogger<UserService> _logger;
        private readonly Models.UserDbContext _context;
        public UserService(ILogger<UserService> logger, UserDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public override Task<userModel> getUser(requestedUser request, ServerCallContext context)
        {
            
            userModel outputUser = new userModel();
            try
            {
                var user = _context.users.Find(request.UserId);

                if (user != null)
                {
                    outputUser.Name = user.username;
                    outputUser.Role = user.role;
                }
                else
                {
                    outputUser.Name = "Not found";
                }
            }
            catch(Exception ex)
            {
                throw;
            }

/*
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=103.4.95.77,11433;User ID=sa;Password=Test123!@#;"))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM [users].[dbo].[users] where id = " + request.UserId, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        outputUser.Name = rdr[1].ToString();
                        outputUser.Password = rdr[2].ToString();
                        outputUser.Role = rdr[3].ToString();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }*/


            /*var user = _context.users.Find(request.UserId);

            if (user != null)
            {
                outputUser.Name = user.username;
                outputUser.Role = user.role;
            }
*/
            return Task.FromResult(outputUser);
        }

        public override async Task getAllUsers(requestedAllUsers request, IServerStreamWriter<userModel> responseStream, ServerCallContext context)
        {
            List<userModel> allUsers = new List<userModel>();

            try
            {

                var users = _context.users.ToList();

                foreach(GrpcGreeter.Models.User u in users)
                {
                    userModel user = new userModel();

                    user.Name = u.username;
                    user.Password = u.password;
                    user.Role = u.role;

                    allUsers.Add(user);
                }
                /*using (SqlConnection con = new SqlConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM [users].[dbo].[users]", con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        userModel user = new userModel();

                        user.Name = rdr[1].ToString();
                        user.Password = rdr[2].ToString();
                        user.Role = rdr[3].ToString();

                        allUsers.Add(user);
                    }
                    con.Close();
                }*/

                foreach (userModel user in allUsers)
                {
                    await responseStream.WriteAsync(user);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /*
                private userModel getValueFromDB(int userId, userModel outputUser)
                {
                    var user = _context.UserDetails.Find(userId);

                    if(user != null)
                    {
                        outputUser.Fname = user.fname;
                        outputUser.Lname = user.lname;
                    }

                    return outputUser;

                }*/
    }
}
