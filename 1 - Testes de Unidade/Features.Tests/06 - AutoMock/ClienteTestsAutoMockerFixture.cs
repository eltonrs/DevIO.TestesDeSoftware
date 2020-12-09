using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Bogus;
using Bogus.DataSets;
using Features.Clientes;
using System.Linq;
using Moq.AutoMock;

namespace Features.Tests
{
  [CollectionDefinition(nameof(ClienteAutoMockerCollection))]

  public class ClienteAutoMockerCollection : ICollectionFixture<ClienteTestsAutoMockerFixture>
  {

  }

  public class ClienteTestsAutoMockerFixture : IDisposable
  {
    public ClienteService clienteService;
    public AutoMocker Mocker;

    public Cliente GerarClienteValido()
    {
      return GerarClientes(1, true).FirstOrDefault();
    }

    public Cliente GerarClienteInvalido()
    {
      var genero = new Faker().PickRandom<Name.Gender>();

      var cliente = new Faker<Cliente>("pt_BR")
        .CustomInstantiator(f => new Cliente(
          Guid.NewGuid(),
          "",
          "",
          f.Date.Past(80, DateTime.Now.AddYears(1)),
          "",
          false,
          DateTime.Now));

      return cliente;
    }

    public IEnumerable<Cliente> ObterClientesVariados()
    {
      var clientes = new List<Cliente>();

      clientes.AddRange(GerarClientes(50, true).ToList());
      clientes.AddRange(GerarClientes(50, false).ToList());

      return clientes;
    }

    public IEnumerable<Cliente> GerarClientes(int quantidade, bool ativo)
    {
      var genero = new Faker().PickRandom<Name.Gender>();

      var clientes = new Faker<Cliente>("pt_BR")
        .CustomInstantiator(f => new Cliente(
          Guid.NewGuid(),
          f.Name.FirstName(genero),
          f.Name.LastName(genero),
          f.Date.Past(80, DateTime.Now.AddYears(-18)),
          "",
          ativo,
          DateTime.Now))
        .RuleFor(c => c.Email, (f, c) =>
          f.Internet.Email(c.Nome.ToLower(), c.Sobrenome.ToLower()));

      return clientes.Generate(quantidade);
    }

    public ClienteService ObterClienteService()
    {
      Mocker = new AutoMocker();
      clienteService = Mocker.CreateInstance<ClienteService>();

      return clienteService;
    }

    public void Dispose()
    {
      //throw new NotImplementedException();
    }
  }
}
