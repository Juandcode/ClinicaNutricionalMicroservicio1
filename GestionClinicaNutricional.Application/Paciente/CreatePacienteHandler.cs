using GestionClinicaNutricional.Domain.Repositories;
using Joseco.DDD.Core.Abstractions;
using Joseco.DDD.Core.Results;
using MediatR;

namespace GestionClinicaNutricional.Application.Paciente
{
    internal class CreatePacienteHandler(IPacienteRepository pacienteRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<CreatePacienteCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreatePacienteCommand request, CancellationToken cancellationToken)
        {
            Domain.Paciente pacienteReq = new Domain.Paciente()
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                CI = request.CI
            };
            await pacienteRepository.AddAsync(pacienteReq);
            await unitOfWork.CommitAsync(cancellationToken);
            return pacienteReq.Id;
        }
    }
}