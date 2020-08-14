using AutoMapper;
using Business.Models;
using Business.QueryModels.Users;
using Data.Queries.Users;
using Data.Repositories;
using System;

namespace Business.Manager
{
    public class UsersManager
    {
        private readonly UserRepository _usersRepository;
        private IMapper _mapper;

        public UsersManager(UserRepository usersRepository,IMapper mappper)
        {
            _usersRepository = usersRepository;
            _mapper = mappper;
        }

        public UsersModel Find(UsersQueryModel queryModel)
        {
            if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));

            var query = _mapper.Map<UsersQuery>(queryModel);
            var entity = _usersRepository.Find(query);

            return _mapper.Map<UsersModel>(entity);
        }
    }
}
