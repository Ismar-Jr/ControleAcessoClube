| SocioId | AreaId | Esperado | GeraException | Descrição                                                                                                            |
| ------- | ------ | -------- | ------------- | -------------------------------------------------------------------------------------------------------------------- |
| 1       | 200    | true     | false         | Sócio ativo, plano ativo, área ativa e permitida. Acesso autorizado.                                                 |
| 1       | 204    | false    | true          | Área inativa. Deve lançar exceção de validação por área inativa.                                                     |
| 2       | 200    | true     | false         | Sócio ativo, plano ativo, área ativa e permitida. Acesso autorizado.                                                 |
| 2       | 202    | false    | false         | Sócio ativo, plano ativo, área ativa, porém área **não permitida** para o plano do sócio. Acesso negado sem exceção. |
| 3       | 200    | true     | false         | Sócio ativo, plano ativo, área ativa e permitida. Acesso autorizado.                                                 |
| 3       | 201    | false    | false         | Sócio ativo, plano ativo, área ativa, porém área não permitida para o plano do sócio. Acesso negado sem exceção.     |
| 3       | 202    | false    | false         | Sócio ativo, plano ativo, área ativa, porém área não permitida para o plano do sócio. Acesso negado sem exceção.     |
| 4       | 200    | false    | true          | Sócio inativo. Deve lançar exceção de validação por sócio inativo.                                                   |
| 5       | 200    | false    | true          | Plano do sócio inativo. Deve lançar exceção de validação por plano inativo.                                          |
| 1       | 999    | false    | true          | Área não existente. Deve lançar exceção de validação por área não encontrada.                                        |
| 9       | 200    | false    | true          | Sócio não existente. Deve lançar exceção de validação por sócio não encontrado.                                      |
| 2       | 204    | false    | true          | Área inativa. Deve lançar exceção de validação por área inativa.                                                     |
