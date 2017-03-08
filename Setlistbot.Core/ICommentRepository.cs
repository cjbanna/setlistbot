using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setlistbot.Core
{
    public interface ICommentRepository
    {
        bool CommentExists(string id);
        void SaveComment(string id, string comment, string reply);
    }
}
