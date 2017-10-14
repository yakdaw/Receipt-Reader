namespace Receipt.API.Model
{
    public interface ISimilarityAlgorithm
    {
        double CompareStrings(string str1, string str2);
    }
}
