﻿namespace MassTransit.WebJobs.EventHubsIntegration.Contexts
{
    using System;
    using System.Threading;
    using Context;
    using EventHubsIntegration;
    using MassTransit.Configuration;
    using Microsoft.Azure.WebJobs;


    public class WebJobEventDataReceiveEndpointContext :
        BaseReceiveEndpointContext
    {
        readonly IBinder _binder;
        readonly CancellationToken _cancellationToken;

        public WebJobEventDataReceiveEndpointContext(IReceiveEndpointConfiguration configuration, Uri inputAddress, IBinder binder,
            CancellationToken cancellationToken)
            : base(configuration)
        {
            _binder = binder;
            _cancellationToken = cancellationToken;

            InputAddress = inputAddress ?? configuration.InputAddress;
        }

        protected override ISendTransportProvider CreateSendTransportProvider()
        {
            return new EventHubAttributeSendTransportProvider(_binder, _cancellationToken);
        }

        protected override IPublishTransportProvider CreatePublishTransportProvider()
        {
            return new EventHubAttributePublishTransportProvider(SendTransportProvider);
        }
    }
}
