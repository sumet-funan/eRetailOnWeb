using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eProductOnWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace eProductOnWeb.Controllers
{
    public class ContainersController : Controller
    {
        private static List<ContainerViewModel> _containerViewModelList = GetContainerViewModelList();


        public IActionResult Index()
        {
            return View(_containerViewModelList);
        }

        public IActionResult Increase(int id, int increaseNumber)
        {
            var containerViewModel = _containerViewModelList.Find(x => x.Id == id);
            containerViewModel.Id += increaseNumber;
            containerViewModel.Name = $"Container {containerViewModel.Id}";
            containerViewModel.Description = $"Container Description {containerViewModel.Id}";
            return View(nameof(Index), _containerViewModelList);
        }

        public IActionResult Delete(int id)
        {
            var containerViewModel = _containerViewModelList.Find(x => x.Id == id);
            _containerViewModelList.Remove(containerViewModel);
            return View(nameof(Index), _containerViewModelList);
        }

        // private method
        private static List<ContainerViewModel> GetContainerViewModelList()
        {
            return new List<ContainerViewModel>
            {
                new ContainerViewModel
                {
                    Id = 1,
                    Name = "Container 1",
                    Description = "Container Description 1",
                },
                new ContainerViewModel
                {
                    Id = 2,
                    Name = "Container 2",
                    Description = "Container Description 2",
                },
                new ContainerViewModel
                {
                    Id = 3,
                    Name = "Container 3",
                    Description = "Container Description 3",
                },
                new ContainerViewModel
                {
                    Id = 4,
                    Name = "Container 4",
                    Description = "Container Description 4",
                },
                new ContainerViewModel
                {
                    Id = 5,
                    Name = "Container 5",
                    Description = "Container Description 5",
                },
                new ContainerViewModel
                {
                    Id = 6,
                    Name = "Container 6",
                    Description = "Container Description 6",
                },
                new ContainerViewModel
                {
                    Id = 7,
                    Name = "Container 7",
                    Description = "Container Description 7",
                },
                new ContainerViewModel
                {
                    Id = 8,
                    Name = "Container 8",
                    Description = "Container Description 8",
                },
                new ContainerViewModel
                {
                    Id = 9,
                    Name = "Container 9",
                    Description = "Container Description 9",
                },
                new ContainerViewModel
                {
                    Id = 10,
                    Name = "Container 10",
                    Description = "Container Description 10",
                },new ContainerViewModel
                {
                    Id = 11,
                    Name = "Container 11",
                    Description = "Container Description 11",
                },
            };
        }
    }
}