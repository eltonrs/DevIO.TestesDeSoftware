using System;
using Bogus;
using static Bogus.DataSets.Name;

namespace BogusBiblioteca
{
  public class BogusLib
  {
    public string GetEmail()
    {
      var nome = new Faker("pt_BR").Name.FirstName();
      var sobrenome = new Faker("pt_BR").Name.LastName();
      var dominio = new Faker().Internet.DomainName();
      var email = new Faker("pt_BR").Internet.Email(nome, sobrenome, dominio);
      
      return $"Nome completo: {nome} {sobrenome}; E-mail: {email}";
    }
  }
}
