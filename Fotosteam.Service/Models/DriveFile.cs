/*
 * Copyright (c) 2012 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
 * in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License
 * is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
 * or implied. See the License for the specific language governing permissions and limitations under
 * the License.
 */

using Google.Apis.Drive.v2.Data;

namespace DrEdit.Models
{
    /// <summary>
    /// Kapselt die Informationen zu
    /// </summary>
    public class DriveFile
    {
        public DriveFile(File file, string content)
        {
            Title = file.Title;
            Description = file.Description;
            MimeType = file.MimeType;
            Content = content;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string MimeType { get; set; }
        public string Content { get; set; }
    }
}