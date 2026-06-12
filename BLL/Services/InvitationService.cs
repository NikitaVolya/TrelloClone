
using BLL.Services.Interface;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork.Interface;
using Domain.Common;
using Domain.Projects;


namespace BLL.Services
{
    public class InvitationService : IInvitationService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public int InvitationDelayInHours => 5;

        public InvitationService(IProjectRepository projectRepository, IInvitationRepository invitationRepository, IAuthService authService, IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _invitationRepository = invitationRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task InviteUserAsync(int projectId, string ownerId, string userId)
        {
            Project? project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new InvalidOperationException($"Project with id '{projectId}' does not exist.");

            if (!(await _authService.UserExistsAsync(userId)))
                throw new InvalidOperationException($"User with id '{userId}' does not exist.");

            if (project.OwnerId != ownerId)
                throw new InvalidOperationException($"User '{ownerId}' is not the owner of project '{projectId}'. Only the owner can send invitations.");

            Invitation? invitation = await _invitationRepository.GetAsync(projectId, userId);
            if (invitation != null)
            {
                if (invitation.Status == InvitationStatus.Pending)
                {
                    throw new InvalidOperationException($"User '{userId}' already has a pending invitation to project '{projectId}'.");
                } 
                else if ((DateTime.UtcNow - invitation.CreatedAt).TotalHours < InvitationDelayInHours)
                {
                    throw new InvalidOperationException($"Invitation for user '{userId}' to project '{projectId}' was recently sent. Please wait before resending.");
                } 
                else
                {
                    invitation.Status = InvitationStatus.Pending;
                    invitation.CreatedAt = DateTime.UtcNow;
                    _invitationRepository.Update(invitation);

                    await _unitOfWork.SaveChangesAsync();   
                }
            } else
            {
                invitation = new Invitation
                {
                    ProjectId = projectId,
                    UserId = userId,
                    Status = InvitationStatus.Pending
                };

                await _invitationRepository.AddAsync(invitation);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<List<Invitation>> GetUserInvitationsAsync(string userId)
        {
            return await _invitationRepository.GetUserInvitationsAsync(userId);
        }

        public async Task AcceptInvitationAsync(int projectId, string userId)
        {
            Project? project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new InvalidOperationException($"Project with id '{projectId}' does not exist.");

            if (!(await _authService.UserExistsAsync(userId)))
                throw new InvalidOperationException($"User with id '{userId}' does not exist.");

            Invitation? invitation = await GetInvitationAsync(projectId, userId);
            if (invitation == null)
                throw new InvalidOperationException($"Invitation for user '{userId}' in project '{projectId}' does not exist.");

            switch (invitation.Status)
            {
                case InvitationStatus.Approved:
                    throw new InvalidOperationException("Invitation has already been accepted.");
                case InvitationStatus.Declined:
                    throw new InvalidOperationException("Invitation has already been declined.");
                case InvitationStatus.Pending:
                    invitation.Status = InvitationStatus.Approved;
                    _invitationRepository.Update(invitation);
                    await _unitOfWork.SaveChangesAsync();
                    break;
            }
        }

        public async Task DeclineInvitationAsync(int projectId, string userId)
        {
            Project? project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new InvalidOperationException($"Project with id '{projectId}' does not exist.");

            if (!(await _authService.UserExistsAsync(userId)))
                throw new InvalidOperationException($"User with id '{userId}' does not exist.");

            Invitation? invitation = await GetInvitationAsync(projectId, userId);
            if (invitation == null)
                throw new InvalidOperationException($"Invitation for user '{userId}' in project '{projectId}' does not exist.");

            switch (invitation.Status)
            {
                case InvitationStatus.Approved:
                    throw new InvalidOperationException("Invitation has already been accepted.");
                case InvitationStatus.Declined:
                    throw new InvalidOperationException("Invitation has already been declined.");
                case InvitationStatus.Pending:
                    invitation.Status = InvitationStatus.Declined;
                    _invitationRepository.Update(invitation);
                    await _unitOfWork.SaveChangesAsync();
                    break;
            }
        }

        public async Task<Invitation?> GetInvitationAsync(int projectId, string userId)
        {
            Project? project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new InvalidOperationException($"Project with id '{projectId}' does not exist.");

            if (!(await _authService.UserExistsAsync(userId)))
                throw new InvalidOperationException($"User with id '{userId}' does not exist.");

            return await _invitationRepository.GetAsync(projectId, userId);
        }
    }
}
