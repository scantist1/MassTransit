namespace MassTransit
{
    using ActiveMqTransport.Scheduling;
    using Registration;
    using Scheduling;
    using Topology;


    public static class ActiveMqMessageSchedulerExtensions
    {
        /// <summary>
        /// Create a message scheduler that uses the built-in ActiveMQ scheduler to schedule messages.
        /// NOTE that this should only be used to schedule messages outside of a message consumer. Consumers should
        /// use the ScheduleSend extensions on ConsumeContext.
        /// </summary>
        /// <param name="bus"></param>
        /// <returns></returns>
        public static IMessageScheduler CreateActiveMqMessageScheduler(this IBus bus)
        {
            return new MessageScheduler(new ActiveMqScheduleMessageProvider(bus), bus.Topology);
        }

        /// <summary>
        /// Create a message scheduler that uses the built-in ActiveMQ scheduler to schedule messages.
        /// NOTE that this should only be used to schedule messages outside of a message consumer. Consumers should
        /// use the ScheduleSend extensions on ConsumeContext.
        /// </summary>
        /// <param name="sendEndpointProvider"></param>
        /// <param name="busTopology"></param>
        /// <returns></returns>
        public static IMessageScheduler CreateActiveMqMessageScheduler(this ISendEndpointProvider sendEndpointProvider, IBusTopology busTopology)
        {
            return new MessageScheduler(new ActiveMqScheduleMessageProvider(sendEndpointProvider), busTopology);
        }

        /// <summary>
        /// Add a <see cref="IMessageScheduler" /> to the container that uses the ActiveMQ scheduler
        /// </summary>
        /// <param name="configurator"></param>
        public static void AddActiveMqMessageScheduler(this IRegistrationConfigurator configurator)
        {
            configurator.AddMessageScheduler(new MessageSchedulerRegistration());
        }


        class MessageSchedulerRegistration :
            IMessageSchedulerRegistration
        {
            public void Register(IContainerRegistrar registrar)
            {
                registrar.Register(provider =>
                {
                    var bus = provider.GetRequiredService<IBus>();
                    var sendEndpointProvider = provider.GetRequiredService<ISendEndpointProvider>();
                    return sendEndpointProvider.CreateActiveMqMessageScheduler(bus.Topology);
                });
            }
        }
    }
}
