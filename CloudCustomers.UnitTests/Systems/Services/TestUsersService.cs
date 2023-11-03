﻿using CloudCustomers.API.Controllers;
using CloudCustomers.API.Models;
using CloudCustomers.API.Services;
using CloudCustomers.UnitTests.Fixtures;
using CloudCustomers.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Threading.Tasks;
using UsersAPI.config;

namespace CloudCustomers.UnitTests.Systems.Services;

public class TestUsersServices
{
    [Fact]
    public async Task GetAllUsers_WhenCalled_InvokesHttpGetRequest()
    {
        //Arrange

        var expectedResponse = UsersFixture.GetTestUsers();
        var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);
        var endpoint = "http://example.com";
        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });

        var sut = new UsersService(httpClient, config);

        //Act

        await sut.GetAllUsers();

        //Assert
        //Verufy that HTTP request is made

        handlerMock
            .Protected()
            .Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>()
            );

    }

    [Fact]

    public async Task GetAllUsers_WhenHits404_ReturnsEmptyListOfUsers()
    {
        //Arrange

        var handlerMock = MockHttpMessageHandler<User>.SetupReturn404();
        var httpClient = new HttpClient(handlerMock.Object);
        var endpoint = "http://example.com";
        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });

        var sut = new UsersService(httpClient, config);

        //Act

        var result = await sut.GetAllUsers();

        //Assert

        result.Count().Should().Be(0);
    }
    [Fact]
    public async Task GetAllUsers_WhenCalled_ReturnsListOfUsers_OfExpectedSize()
    {
        //Arrange

        var expectedResponse = UsersFixture.GetTestUsers();
        var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse);
        var httpClient = new HttpClient(handlerMock.Object);
        var endpoint = "http://example.com";
        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });

        var sut = new UsersService(httpClient, config);

        //Act

        var result = await sut.GetAllUsers();

        //Assert

        result.Count().Should().Be(expectedResponse.Count);

    }

    [Fact]
    public async Task GetAllUsers_WhenCalled_InvokesConfiguredExternalUrl()
    {
        //Arrange

        var expectedResponse = UsersFixture.GetTestUsers();
        var endpoint = "http://example.com";
        var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse, endpoint);
        var httpClient = new HttpClient(handlerMock.Object);

        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });

        var sut = new UsersService(httpClient, config);

        //Act

        var result = await sut.GetAllUsers();
        var uri = new Uri(endpoint);

        //Assert

        handlerMock
            .Protected()
            .Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == uri),
            ItExpr.IsAny<CancellationToken>()
            );

    }
}