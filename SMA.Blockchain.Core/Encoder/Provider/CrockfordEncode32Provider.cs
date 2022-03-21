namespace SMA.Blockchain.Core.Encoder.Provider;
internal sealed class CrockfordEncode32Provider : Encode32Provider
{
    public CrockfordEncode32Provider()
        : base("0123456789ABCDEFGHJKMNPQRSTVWXYZ")
    {
        this.mapAlternate('O', '0');
        this.mapAlternate('I', '1');
        this.mapAlternate('L', '1');
    }

    private void mapAlternate(char source, char destination)
    {
        int result = this.ReverseLookupTable[destination] - 1;
        this.Map(source, result);
        this.Map(char.ToLowerInvariant(source), result);
    }
}
