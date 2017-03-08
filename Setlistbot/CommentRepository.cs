using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Setlistbot.Core;

namespace Setlistbot
{
    public class CommentRepository : ICommentRepository
    {
        private string _partitionKey;
        private CloudTable _comments;

        public CommentRepository(string partitionKey)
        {
            _partitionKey = partitionKey;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            _comments = tableClient.GetTableReference("comments");
            if (!_comments.Exists())
            {
                _comments.Create();
            }
        }

        public bool CommentExists(string id)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<CommentEntity>(_partitionKey, id);
            TableResult retrievedResult = _comments.Execute(retrieveOperation);
            return retrievedResult.Result != null;
        }

        public void SaveComment(string id, string comment, string reply)
        {
            CommentEntity entity = new CommentEntity(_partitionKey, id, comment, reply);
            TableOperation insert = TableOperation.Insert(entity);
            _comments.Execute(insert);
        }
    }

    public class CommentEntity : TableEntity
    {
        public CommentEntity() { }

        public CommentEntity(string partitionKey, string id, string comment, string reply)
        {
            PartitionKey = partitionKey;
            RowKey = id;
            Comment = comment;
            Reply = reply;
        }

        public string Comment { get; set; }
        public string Reply { get; set; }
    }
}
