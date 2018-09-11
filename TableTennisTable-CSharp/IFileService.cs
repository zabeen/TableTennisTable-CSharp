using System;

namespace TableTennisTable_CSharp
{
    public interface IFileService
    {
        void Save(string path, League league);
        League Load(string path);
    }
}
