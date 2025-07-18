﻿namespace ControleAcesso.Aplicacao.Servicos.Criptografia;

public class CriptografiaDeSenha
{
    public string Encrypt(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    public bool Verify(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}