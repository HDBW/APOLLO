// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Helper;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class AnswerEntry : ObservableObject
    {
        private readonly AnswerItem _answerItem;
        private readonly IEnumerable<MetaDataItem> _answerMetaDataItems;
        private readonly AnswerItemResult _answerItemResult;
        private Point? _point;

        private AnswerEntry(AnswerItem answerItem, AnswerItemResult answerItemResult, IEnumerable<MetaDataItem> answerMetaDataItems)
        {
            ArgumentNullException.ThrowIfNull(answerItem);
            ArgumentNullException.ThrowIfNull(answerMetaDataItems);
            _answerItem = answerItem;
            _answerMetaDataItems = answerMetaDataItems;
            _answerItemResult = answerItemResult;
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
                return _answerMetaDataItems.FirstOrDefault(m => m.Type == MetaDataType.Text)?.Value ?? string.Empty;
            }
        }

        public bool HasHint
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Hint);
            }
        }

        public string Hint
        {
            get
            {
                return _answerMetaDataItems.FirstOrDefault(m => m.Type == MetaDataType.Hint)?.Value ?? string.Empty;
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
                return _answerMetaDataItems.FirstOrDefault(m => m.Type == MetaDataType.Image)?.Value?.ToUniformedName();
            }
        }

        public bool HasPoint
        {
            get
            {
                return Position.HasValue;
            }
        }

        public Point? Position
        {
            get
            {
                if (AnswerType != AnswerType.Location)
                {
                    return null;
                }

                if (_point != null)
                {
                    return _point;
                }

                var point = _answerMetaDataItems.FirstOrDefault(m => m.Type == MetaDataType.Point2D)?.Value ?? string.Empty;
                if (string.IsNullOrWhiteSpace(point))
                {
                    return null;
                }

                var positions = point.Split(";");
                if (positions.Length != 2)
                {
                    return null;
                }

                if (!double.TryParse(positions[0], out double x) ||
                    !double.TryParse(positions[1], out double y) ||
                    double.IsInfinity(x) ||
                    double.IsNaN(x) ||
                    double.IsInfinity(y) ||
                    double.IsNaN(y))
                {
                    return null;
                }

                _point = new Point(x, y);
                return _point;
            }
        }

        public AnswerType AnswerType
        {
            get
            {
                return _answerItem.AnswerType;
            }
        }

        public AnswerItemResult Result
        {
            get
            {
                return _answerItemResult;
            }
        }

        public long Id
        {
            get
            {
                return _answerItem.Id;
            }
        }

        public string Value
        {
            get
            {
                return _answerItem.Value;
            }
        }

        public string CurrentValue
        {
            get
            {
                return _answerItemResult.Value;
            }

            set
            {
                _answerItemResult.Value = value;
            }
        }

        public bool IsCorrect
        {
            get
            {
                return string.Equals(Value, CurrentValue);
            }
        }

        public static AnswerEntry Import(AnswerItem answerItem, AnswerItemResult answerItemResult, IEnumerable<MetaDataItem> answerMetaDataItems)
        {
            return new AnswerEntry(answerItem, answerItemResult, answerMetaDataItems);
        }

        public AnswerItemResult ExportResult()
        {
            return _answerItemResult;
        }
    }
}
