// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Helper;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class QuestionDetailEntry : BindableObject
    {
        private readonly IEnumerable<MetaDataItem> _metaDatas;

        public QuestionDetailEntry(IEnumerable<MetaDataItem> metadatas)
        {
            ArgumentNullException.ThrowIfNull(metadatas);
            _metaDatas = metadatas;
        }

        public bool HasText
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Text);
            }
        }

        public string Text
        {
            get
            {
                return _metaDatas.FirstOrDefault(m => m.Type == MetaDataType.Text)?.Value ?? string.Empty;
            }
        }

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ImagePath);
            }
        }

        public string? ImagePath
        {
            get
            {
                return _metaDatas.FirstOrDefault(m => m.Type == MetaDataType.Image)?.Value?.ToUniformedName();
            }
        }

        public long Id
        {
            get
            {
                return _metaDatas.FirstOrDefault(m => m.Type == MetaDataType.Image)?.Id ?? 0;
            }
        }

        public static QuestionDetailEntry Import(IEnumerable<MetaDataItem> metadatas)
        {
            return new QuestionDetailEntry(metadatas);
        }
    }
}
