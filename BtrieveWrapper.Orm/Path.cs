using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class Path
    {
        Path(PathType pathType) {
            this.PathType = pathType;
            this.IsMergeable = true;
        }

        public static Path Uri(
            string uriHost = null,
            string uriDbName = null,
            string uriTable = null,
            string uriDbFile = null,
            string uriFile = null,
            string uriUser = null,
            string uriPassword = null,
            bool? uriPrompt = null,
            bool mergeable = true) {

            var result = new Path(PathType.Uri);
            result.UriHost = uriHost;
            result.UriDbName = uriDbName;
            result.UriTable = uriTable;
            result.UriDbFile = uriDbFile;
            result.UriFile = uriFile;
            result.UriUser = uriUser;
            result.UriPassword = uriPassword;
            result.UriPrompt = uriPrompt;
            result.IsMergeable = mergeable;
            return result;
        }

        public static Path Relative(string relativePath = null, string relativeDirectory = null, bool mergeable = true) {
            var result = new Path(PathType.Relative);
            result.RelativePath = relativePath;
            result.RelativeDirectory = relativeDirectory;
            result.IsMergeable = mergeable;
            return result;
        }

        public static Path Directory(string relativeDirectory) {
            var result = new Path(PathType.Relative);
            result.RelativeDirectory = relativeDirectory;
            return result;
        }

        public static Path Absolute(string absolutePath = null) {
            var result = new Path(PathType.Absolute);
            result.AbsolutePath = absolutePath;
            return result;
        }

        public PathType PathType { get; set; }
        public string UriHost { get; set; }
        public string UriDbName { get; set; }
        public string UriTable { get; set; }
        public string UriDbFile { get; set; }
        public string UriFile { get; set; }
        public string UriUser { get; set; }
        public string UriPassword { get; set; }
        public bool? UriPrompt { get; set; }
        public string AbsolutePath { get; set; }
        public string RelativePath { get; set; }
        public string RelativeDirectory { get; set; }

        public bool IsMergeable { get; set; }

        public string GetFilePath() {
            switch (this.PathType) {
                case Orm.PathType.Uri:
                    if (String.IsNullOrEmpty(this.UriHost)) {
                        throw new InvalidOperationException();
                    }

                    var uri = new StringBuilder("btrv://");
                    if (this.UriUser != null) {
                        uri.Append(this.UriUser);
                        uri.Append("@");
                    }
                    uri.Append(this.UriHost);
                    uri.Append("/");
                    uri.Append(this.UriDbName ?? "");
                    if (this.UriTable != null ||
                        this.UriDbFile != null ||
                        this.UriFile != null ||
                        this.UriPassword != null ||
                        this.UriPrompt != null) {
                        uri.Append("?");
                        if (this.UriTable != null) {
                            uri.Append("table=");
                            uri.Append(this.UriTable);
                            uri.Append("&");
                        }
                        if (this.UriDbFile != null) {
                            uri.Append("dbfile=");
                            uri.Append(this.UriDbFile);
                            uri.Append("&");
                        }
                        if (this.UriFile != null) {
                            uri.Append("file=");
                            uri.Append(this.UriFile);
                            uri.Append("&");
                        }
                        if (this.UriPassword != null) {
                            uri.Append("pwd=");
                            uri.Append(this.UriPassword);
                            uri.Append("&");
                        }
                        if (this.UriPrompt != null) {
                            uri.Append("prompt=");
                            uri.Append(this.UriPrompt.Value ? "yes" : "no");
                            uri.Append("&");
                        }
                        uri.Remove(uri.Length - 1, 1);
                    }
                    return uri.ToString();
                case Orm.PathType.Absolute:
                    if (this.AbsolutePath == null) {
                        throw new InvalidOperationException();
                    }
                    return this.AbsolutePath;
                case Orm.PathType.Relative:
                    if (this.RelativePath == null) {
                        throw new InvalidOperationException();
                    }
                    var directory = this.RelativeDirectory ?? Environment.CurrentDirectory;
                    return System.IO.Path.Combine(directory, this.RelativePath);
                default:
                    throw new InvalidOperationException();
            }
        }

        public static Path Merge(Path path, RecordInfo recordInfo) {
            if (path == null) {
                path = new Path(recordInfo.PathType);
            } else {
                path = path.DeepCopy();
                if (!path.IsMergeable) {
                    return path;
                }
            }
            path.UriHost = path.UriHost ?? recordInfo.UriHost;
            path.UriDbName = path.UriDbName ?? recordInfo.UriDbName;
            path.UriTable = path.UriTable ?? recordInfo.UriTable;
            path.UriDbFile = path.UriDbFile ?? recordInfo.UriDbFile;
            path.UriFile = path.UriFile ?? recordInfo.UriFile;
            path.UriUser = path.UriUser ?? recordInfo.UriUser;
            path.UriPassword = path.UriPassword ?? recordInfo.UriPassword;
            path.UriPrompt = path.UriPrompt ?? recordInfo.UriPrompt;
            path.AbsolutePath = path.AbsolutePath ?? recordInfo.AbsolutePath;
            path.RelativePath = path.RelativePath ?? recordInfo.RelativePath;
            path.RelativeDirectory = path.RelativeDirectory ?? recordInfo.RelativeDirectory;
            return path;
        }

        public static Path Merge(Path path1, Path path2) {
            if (path1 == null) {
                return path2.DeepCopy();
            }
            if (path2 == null || !path1.IsMergeable) {
                return path1.DeepCopy();
            }
            var path = path1.DeepCopy();
            path.UriHost = path1.UriHost ?? path2.UriHost;
            path.UriDbName = path1.UriDbName ?? path2.UriDbName;
            path.UriTable = path1.UriTable ?? path2.UriTable;
            path.UriDbFile = path1.UriDbFile ?? path2.UriDbFile;
            path.UriFile = path1.UriFile ?? path2.UriFile;
            path.UriUser = path1.UriUser ?? path2.UriUser;
            path.UriPassword = path1.UriPassword ?? path2.UriPassword;
            path.UriPrompt = path1.UriPrompt ?? path2.UriPrompt;
            path.AbsolutePath = path1.AbsolutePath ?? path2.AbsolutePath;
            path.RelativePath = path1.RelativePath ?? path2.RelativePath;
            path.RelativeDirectory = path1.RelativeDirectory ?? path2.RelativeDirectory;
            return path;
        }

        public Path DeepCopy() {
            var result = new Path(this.PathType);
            result.UriHost = this.UriHost;
            result.UriDbName = this.UriDbName;
            result.UriTable = this.UriTable;
            result.UriDbFile = this.UriDbFile;
            result.UriFile = this.UriFile;
            result.UriUser = this.UriUser;
            result.UriPassword = this.UriPassword;
            result.UriPrompt = this.UriPrompt;
            result.AbsolutePath = this.AbsolutePath;
            result.RelativePath = this.RelativePath;
            result.RelativeDirectory = this.RelativeDirectory;
            result.IsMergeable = this.IsMergeable;
            return result;
        }
    }
}
