// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Invite.Apollo.App.Graph.Common.Models;
using Invite.Apollo.App.Graph.Common.Models.Assessment;
using Invite.Apollo.App.Graph.Common.Models.Assessment.Enums;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public partial class AnswerEntry : ObservableObject
    {
        private AnswerItem _answerItem;
        private IEnumerable<MetaDataItem> _answerMetaDataItems;
        private Point? _point;

        private AnswerEntry(AnswerItem answerItem, IEnumerable<MetaDataItem> answerMetaDataItems)
        {
            ArgumentNullException.ThrowIfNull(answerItem);
            ArgumentNullException.ThrowIfNull(answerMetaDataItems);
            _answerItem = answerItem;
            _answerMetaDataItems = answerMetaDataItems;
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

        public bool HasImage
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Image);
            }
        }

        public string Image
        {
            get
            {
                return _answerMetaDataItems.FirstOrDefault(m => m.Type == MetaDataType.Image)?.Value ?? string.Empty;
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

        public static AnswerEntry Import(AnswerItem answerItem, IEnumerable<MetaDataItem> answerMetaDataItems)
        {
            return new AnswerEntry(answerItem, answerMetaDataItems);
        }
    }
}
