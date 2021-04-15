using AirClub.Model;
using AirClub.Model.Access;
using AirClub.Model.Db;
using AirClub.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirClub.Services
{
    public interface ICurrentUserService
    {
        public Employee CurrentUser { get; }

        public bool IsEmptyDb { get; }

        public Task OnUserEnter(Employee emp, int retrCount);

        public void OnUserExit();

        public event Action<Employee> UserEntered;

        public event Action UserExited;
        public int? GetUserPossibility(AccessSectionsIndex sectionIndex);
        IEnumerable<IUserAccessOnSection> UserAccesses { get; }
        IList<IAccessSection> AccessSections { get; }
        Task UpdateUser(Employee emp);
        void DeserializeAccessSections();

        Task OnItemsViewModelOpen<T>(ElementsBaseViewModel<T> vm) where T : class, IDbClass, new();
        Task<IEnumerable<UserTableOpenAction>> GetUserTableActions();
    }


    public class CurrentUserService : ICurrentUserService
    {
        private readonly AirClubDbContext _dbContext;

        public Employee CurrentUser { get; private set; }

        public event Action<Employee> UserEntered;
        public event Action UserExited;
        public IEnumerable<IUserAccessOnSection> UserAccesses { get; private set; }

        public IList<IAccessSection> AccessSections { get; private set; }

        public bool IsEmptyDb { get; } = true;

        public async Task<IEnumerable<UserTableOpenAction>> GetUserTableActions()
        {
            await _dbContext.UserTableOpenActions.LoadAsync();
            return _dbContext.UserTableOpenActions.Where(x => x.EmloyeeId == CurrentUser.Id);
        }

        public CurrentUserService(AirClubDbContext dbContext)
        {
            DeserializeAccessSections();
            _dbContext = dbContext;
        }

        public int? GetUserPossibility(AccessSectionsIndex sectionIndex)
        {
            var element = UserAccesses?.FirstOrDefault(x => x.AccessIndex == (int)sectionIndex);


            if (IsEmptyDb && AccessCodeConverter.GetMax(AccessSections, sectionIndex, out int res))
            {
                return res;
            }

            if(element != null)
            {
                return element.Value;
            }
            return null;
            //if(sectionIndex == AccessSectionsIndex.AddEmployee || sectionIndex == AccessSectionsIndex.AddSpecial)
            //{
            //    return 1;
            //}
            //return 6;
        }

        public async Task UpdateUser(Employee emp)
        {
            CurrentUser = emp;
            UserAccesses = (await AccessCodeConverter.ConvertFrom(emp.AccessCode, AccessSections.ToList())).ToList();
        }

        public async Task OnUserEnter(Employee emp, int retrCount)
        {
            _dbContext.UserEnterActions.Add(
                    new UserEnterAction
                    {
                        DateAction = DateTime.Now,
                        EmloyeeId = emp.Id,
                        NumberOfRertyes = retrCount,
                    });

            await Task.Run(() => _dbContext.SaveChangesAsync());

            await UpdateUser(emp);
            UserEntered?.Invoke(emp);
        }

        public void OnUserExit()
        {
            //UserAccesses = null;
            CurrentUser = null;

            UserExited?.Invoke();
        }

        public void DeserializeAccessSections()
        {
            AccessSections = AccessCodeConverter.Deserialize().ToList();
        }

        public async Task OnItemsViewModelOpen<T>(ElementsBaseViewModel<T> vm) where T : class, IDbClass, new()
        {
            string name = vm.GetType().FullName;

            var action = new UserTableOpenAction
            {
                DateAction = DateTime.Now,
                EmloyeeId = CurrentUser.Id,
                VmType = name,
                DbClassType = (typeof (T)).FullName,
            };
            _dbContext.UserTableOpenActions.Add(action);
            await Task.Run(() => _dbContext.SaveChangesAsync());
        }
    }
}
