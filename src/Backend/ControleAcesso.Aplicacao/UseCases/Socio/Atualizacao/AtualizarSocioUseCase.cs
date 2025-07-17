using AutoMapper;
using ControleAcesso.Comunicacao.Requisicoes;
using ControleAcesso.Dominio.Repositorios;
using ControleAcesso.Dominio.Repositorios.Socio;
using ControleAcesso.Excecoes.ExceptionsBase;
using FluentValidation.Results;
using System.Linq;
using System.Threading.Tasks;
using ControleAcesso.Aplicacao.UseCases.Socio.Registro;

namespace ControleAcesso.Aplicacao.UseCases.Socio.Atualizacao
{
    public class AtualizarSocioUseCase : IAtualizarSocioUseCase
    {
        private readonly ISocioReadOnlyRepositorio _readOnlyRepositorio;
        private readonly ISocioWriteOnlyRepositorio _writeOnlyRepositorio;
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
        private readonly IMapper _mapper;

        public AtualizarSocioUseCase(
            ISocioReadOnlyRepositorio readOnlyRepositorio,
            ISocioWriteOnlyRepositorio writeOnlyRepositorio,
            IUnidadeDeTrabalho unidadeDeTrabalho,
            IMapper mapper)
        {
            _readOnlyRepositorio = readOnlyRepositorio;
            _writeOnlyRepositorio = writeOnlyRepositorio;
            _unidadeDeTrabalho = unidadeDeTrabalho;
            _mapper = mapper;
        }

        public async Task Execute(long id, RequisicaoRegistroSocioJson request)
        {
            var socio = await _readOnlyRepositorio.ObterPorIdAsync(id);

            if (socio == null || !socio.Ativo)
                throw new ErroDeValidacao(new[] { "Sócio não encontrado ou inativo." });

            var validador = new ValidacaoRegistroSocio(); // se necessário, você pode criar esse validador separado
            var resultado = validador.Validate(request);

            if (!resultado.IsValid)
            {
                var mensagensErro = resultado.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErroDeValidacao(mensagensErro);
            }

            socio.Nome = request.Nome;
            socio.Cpf = request.Cpf;
            socio.Email = request.Email;

            await _writeOnlyRepositorio.AtualizarAsync(socio);
            await _unidadeDeTrabalho.Commit();
        }
    }
}
