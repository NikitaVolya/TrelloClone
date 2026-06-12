
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
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectService _projectService;

        public int InvitationDelayInHours => 5;

        public InvitationService(IProjectRepository projectRepository, IInvitationRepository invitationRepository, IUserService userService, IUnitOfWork unitOfWork, IProjectService projectService)
        {
            _projectRepository = projectRepository;
            _invitationRepository = invitationRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _projectService = projectService;
        }

        public async Task InviteUserAsync(int projectId, string ownerId, string userId)
        {
            Project? project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new InvalidOperationException($"Project with id '{projectId}' does not exist.");

            if (!(await _userService.UserExistsAsync(userId)))
                throw new InvalidOperationException($"User with id '{userId}' does not exist.");

            if (project.OwnerId != ownerId)
                throw new InvalidOperationException($"User '{ownerId}' is not the owner of project '{projectId}'. Only the owner can send invitations.");

            
            Invitation invitation = new Invitation
            {
                ProjectId = projectId,
                UserId = userId,
                Status = InvitationStatus.Pending,
                CreatedAt = DateTime.UtcNow,
            };

            await _invitationRepository.AddAsync(invitation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<Invitation>> GetUserInvitationsAsync(string userId)
        {
            return await _invitationRepository.GetUserInvitationsAsync(userId);
        }

        public async Task AcceptInvitationAsync(int invitationId, string userId)
        {
            Invitation? invitation = await _invitationRepository.GetInvitationByIdAsync(invitationId);
            if (invitation == null)
                throw new InvalidOperationException($"Invitation does not exist.");

            if (invitation.UserId != userId)
                throw new InvalidOperationException($"Invitation can not be acceped by user with id {userId}");

            switch (invitation.Status)
            {
                case InvitationStatus.Approved:
                    throw new InvalidOperationException("Invitation has already been accepted.");
                case InvitationStatus.Declined:
                    throw new InvalidOperationException("Invitation has already been declined.");
                case InvitationStatus.Pending:
                    invitation.Status = InvitationStatus.Approved;
                    _invitationRepository.Update(invitation);
                    await _projectService.AddMemberAsync(invitation.ProjectId, invitation.UserId);
                    await _unitOfWork.SaveChangesAsync();
                    break;
            }
        }

        public async Task DeclineInvitationAsync(int invitationId, string userId)
        {
            Invitation? invitation = await _invitationRepository.GetInvitationByIdAsync(invitationId);
            if (invitation == null)
                throw new InvalidOperationException($"Invitation does not exist.");

            if (invitation.UserId != userId)
                throw new InvalidOperationException($"Invitation can not be acceped by user with id {userId}");

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

        public async Task<Invitation?> GetInvitationAsync(int invitationId)
        {
            return await _invitationRepository.GetInvitationByIdAsync(invitationId);
        }
    }
}
