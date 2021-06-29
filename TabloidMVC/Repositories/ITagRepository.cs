using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAll();
        void AddTag(Tag tag);
        void DeleteTag(int id);
        void EditTag(Tag tag);
        Tag GetTagById(int id);
    }
}
