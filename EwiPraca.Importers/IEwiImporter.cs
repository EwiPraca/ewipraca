namespace EwiPraca.Importers
{
    public interface IEwiImporter
    {
        void Import(string fileName, int companyId);
    }
}
