using System;
using Microsoft.Extensions.Options;

namespace UserSecrets.Console
{
    public class SecretRevealer : ISecretRevealer
    {
        private readonly SecretStuff _secrets;
        public SecretRevealer (IOptions<SecretStuff> secrets)
        {
            _secrets = secrets.Value ?? throw new ArgumentNullException(nameof(secrets));
        }

        public void Reveal()
        {
            //I can now use my mapped secrets below.
            System.Console.WriteLine($"Secret One: {_secrets.SecretOne}");
            System.Console.WriteLine($"Secret Two: {_secrets.SecretTwo}");
        }
    }
}
