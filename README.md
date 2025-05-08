# 📈 Measurement Generator

Este projeto é um serviço backend em **.NET 8**, com execução periódica, que lê medições de arquivos `.csv`, simula medições se necessário e armazena os dados no banco de dados **SQL Server**. Ideal para integrações com sensores de pressão, vazão e shutoff em plantas industriais.

## 🛠️ Tecnologias Utilizadas

- .NET 8
- Entity Framework Core
- SQL Server 2022
- CsvHelper
- Docker + Docker Compose

---

## 📋 Funcionalidades

- ⏰ Executa a cada 15 minutos de forma automática.
- 📦 Lê arquivos `.csv` de medições com timestamp.
- 🔄 Simula dados ausentes com variação de ±10%.
- 💾 Armazena as medições mais recentes no banco.
- 🔍 API para registrar ERPs, listar, e verificar arquivos disponíveis.

---

## 🔌 Endpoints da API

| Rota                       | Método | Descrição                                        |
|---------------------------|--------|--------------------------------------------------|
| `/erps/register`          | GET    | Registra os ERPs a partir de `ErpCadastradas.csv` |
| `/erps/all`               | GET    | Lista todos os ERPs cadastrados                 |
| `/erps/csv`               | GET    | Verifica quais ERPs possuem arquivos `.csv`     |
| `/erps/measurements`      | GET    | Lê medições dos arquivos de acordo com a data   |

---

## 🐳 Docker

Este projeto está preparado para rodar com Docker utilizando Docker Compose.

### Pré-requisitos

- Docker
- Docker Compose

### Subir os serviços

```bash
docker-compose up --build
````

### Serviços

* **SQL Server** — Porta `1433`
* **Measurement Generator (.NET)** — Porta `1111`

### Estrutura de Volumes

* Os arquivos de medições `.csv` devem estar disponíveis no host e montados no container na pasta:

  ```
  /home/measurements-comgas
  ```

> 📝 No Windows, você pode ajustar a linha no `docker-compose.yml`:

```yaml
volumes:
  - C:\Users\seu_usuario\Desktop\measurements-comgas:/app/data
```

---

## 📂 Organização de Arquivos Esperados

### 1. Arquivo de ERPs

Coloque em:
`/app/assets/ErpCadastradas.csv`

Formato exemplo:

```csv
ERP_Cod,Type,ScopeId,Lat,Lon
erp1,TipoA,123,-23.5,-46.6
erp2,TipoB,124,-23.6,-46.5
```

### 2. Arquivos de Medições

Coloque em:
`/home/measurements-comgas/erp1.csv`
`/home/measurements-comgas/erp2.csv`

Formato esperado:

```csv
dd/MM/yyyy HH:mm:ss,piLL,piHL,pi,poLL,poHL,po,z1,z2,flow,pdt1,pdt2,r1,r2
21/04/2024 18:30:00,1.0,2.0,1.5,0.9,1.8,1.2,1.1,1.2,10,0.5,0.6,0.8,0.9
```

---

## ⏱️ Execução Agendada

* O serviço principal (`ScheduledTaskService`) executa a cada 15 minutos exatamente (00, 15, 30, 45).
* A cada execução:

  * Simula uma data em 2023 para leitura dos arquivos (útil para dados históricos).
  * Lê ou simula os dados das medições por ERP.
  * Atualiza os dados na tabela `LastMeasurements`.

---

## 🧪 Testes e Monitoramento

* Logs básicos são escritos no console para:

  * Início de execução agendada
  * Leitura de arquivos
  * Detecção de erro ou ausência de dados

---

## 📃 Licença

Este projeto é de uso interno. Sinta-se livre para modificar e adaptar conforme necessário.

---

## 👨‍💻 Autor

Desenvolvido por Thales .
Entre em contato em: thalesmoreirax@gmail.com
```

