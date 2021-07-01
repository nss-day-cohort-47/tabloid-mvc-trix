using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        public List<Comment> GetCommentsById(int id);
        public void AddComment(Comment comment);
        //public void UpdateComment(Comment comment);
        //public void DeleteComment(int id);
        //public Comment GetCommentById(int id);
    }
}
