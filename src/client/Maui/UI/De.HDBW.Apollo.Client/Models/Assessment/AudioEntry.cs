// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using Audio = De.HDBW.Apollo.SharedContracts.Models.Audio;

namespace De.HDBW.Apollo.Client.Models.Assessment
{
    public class AudioEntry : ObservableObject
    {
        private readonly Audio _data;

        private readonly string _basePath;

        private AudioEntry(Audio data, string basePath)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(basePath);
            _data = data;
            _basePath = basePath;
        }

        public string AbsolutePath
        {
            get
            {
                return Path.Combine(_basePath, _data.id, _data.name);
            }
        }

        public static AudioEntry Import(Audio data, string basePath)
        {
            return new AudioEntry(data, basePath);
        }
    }
}
